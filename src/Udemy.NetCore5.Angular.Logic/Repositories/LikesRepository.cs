using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Extensions;
using Udemy.NetCore5.Angular.Logic.Helpers;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Logic.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId).ConfigureAwait(false);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(l => l.LikedUsers)
                .FirstOrDefaultAsync(u => u.Id == userId)
                .ConfigureAwait(false);
        }

        public async Task<PagedList<AppUserLikesResponse>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            switch (likesParams.Predicate)
            {
                case "liked":
                {
                    likes = likes.Where(l => l.SourceUserId == likesParams.UserId);
                    users = likes.Select(l => l.LikedUser);
                }
                    break;
                case "likedBy":
                {
                    likes = likes.Where(l => l.LikedUserId == likesParams.UserId);
                    users = likes.Select(l => l.SourceUser);
                }
                    break;
            }

            var likedUsers = users.Select(u => new AppUserLikesResponse
            {
                UserName = u.UserName,
                KnownAs = u.KnownAs,
                Age = u.DateOfBirth.CalculateAge(),
                PhotoUrl = u.Photos.FirstOrDefault(p => p.Enabled).Url,
                City = u.City,
                Id = u.Id
            });

            return await PagedList<AppUserLikesResponse>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }
    }
}
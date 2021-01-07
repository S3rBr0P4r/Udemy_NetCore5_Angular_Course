using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Helpers;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Logic.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AppUserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllChangesAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<PagedList<AppUserResponse>> GetUsersAsync(UserParams userParams)
        {
            var query = _context.Users
                .AsQueryable()
                .Where(u =>
                    u.UserName != userParams.CurrentUserName &&
                    u.Gender == userParams.Gender &&
                    u.DateOfBirth >= DateTime.Today.AddYears(-userParams.MaxAge - 1) &&
                    u.DateOfBirth <= DateTime.Today.AddYears(-userParams.MinAge));

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.UserCreated),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<AppUserResponse>.CreateAsync(
                query.ProjectTo<AppUserResponse>(_mapper.ConfigurationProvider).AsNoTracking(),
                userParams.PageNumber, userParams.PageSize)
                .ConfigureAwait(false);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Include(p => p.Photos)
                .SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<AppUserResponse> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .Where(u => u.UserName == userName)
                .ProjectTo<AppUserResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
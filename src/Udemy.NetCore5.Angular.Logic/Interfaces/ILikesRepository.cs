using System.Collections.Generic;
using System.Threading.Tasks;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;

namespace Udemy.NetCore5.Angular.Logic.Interfaces
{
    public interface ILikesRepository
    {
        Task<AppUserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<IEnumerable<AppUserLikesResponse>> GetUserLikes(string predicate, int userId);
    }
}
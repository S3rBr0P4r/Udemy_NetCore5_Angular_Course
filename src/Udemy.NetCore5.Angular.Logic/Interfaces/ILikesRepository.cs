using System.Threading.Tasks;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Helpers;

namespace Udemy.NetCore5.Angular.Logic.Interfaces
{
    public interface ILikesRepository
    {
        Task<AppUserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<AppUserLikesResponse>> GetUserLikes(LikesParams likesParams);
    }
}
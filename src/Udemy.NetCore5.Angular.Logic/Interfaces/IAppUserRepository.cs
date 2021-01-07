using System.Threading.Tasks;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;
using Udemy.NetCore5.Angular.Logic.Helpers;

namespace Udemy.NetCore5.Angular.Logic.Interfaces
{
    public interface IAppUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllChangesAsync();

        Task<PagedList<AppUserResponse>> GetUsersAsync(UserParams userParams);

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUserResponse> GetUserByUserNameAsync(string userName);
    }
}
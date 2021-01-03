using System.Collections.Generic;
using System.Threading.Tasks;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.DTOs;

namespace Udemy.NetCore5.Angular.Logic.Interfaces
{
    public interface IAppUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllChangesAsync();

        Task<IEnumerable<AppUserResponse>> GetUsersAsync();

        Task<AppUserResponse> GetUserByIdAsync(int id);

        Task<AppUserResponse> GetUserByUserNameAsync(string userName);
    }
}
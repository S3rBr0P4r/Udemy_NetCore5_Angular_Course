using Udemy.NetCore5.Angular.Data.Entities;

namespace Udemy.NetCore5.Angular.Logic.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
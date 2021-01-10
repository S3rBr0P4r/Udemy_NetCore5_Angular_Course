using Microsoft.AspNetCore.Identity;

namespace Udemy.NetCore5.Angular.Data.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }

        public AppRole Role { get; set; }
    }
}
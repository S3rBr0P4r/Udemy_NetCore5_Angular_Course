using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Udemy.NetCore5.Angular.Data.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
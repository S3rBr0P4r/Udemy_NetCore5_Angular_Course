using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data.Entities;

namespace Udemy.NetCore5.Angular.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync().ConfigureAwait(false))
            {
                return;
            }

            var userData = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserSeedData.json"));
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null)
            {
                return;
            }

            var roles = new List<AppRole>
            {
                new AppRole {Name = "Member"},
                new AppRole {Name = "Admin"},
                new AppRole {Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role).ConfigureAwait(false);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLowerInvariant();

                await userManager.CreateAsync(user, "123456").ConfigureAwait(false);
                await userManager.AddToRoleAsync(user, "Member").ConfigureAwait(false);
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "123456").ConfigureAwait(false);
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
        }
    }
}
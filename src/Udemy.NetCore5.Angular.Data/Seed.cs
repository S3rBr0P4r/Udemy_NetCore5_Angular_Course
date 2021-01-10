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
        public static async Task SeedUsers(UserManager<AppUser> userManager)
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

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLowerInvariant();

                await userManager.CreateAsync(user, "123456").ConfigureAwait(false);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data.Entities;

namespace Udemy.NetCore5.Angular.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync().ConfigureAwait(false))
            {
                return;
            }

            var userData = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserSeedData.json"));
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLowerInvariant();

                await context.Users.AddAsync(user).ConfigureAwait(false);
            }

            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
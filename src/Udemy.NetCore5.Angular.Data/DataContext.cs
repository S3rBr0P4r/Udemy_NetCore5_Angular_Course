using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data.Entities;

namespace Udemy.NetCore5.Angular.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<AppUser> Users { get; set; }
    }
}
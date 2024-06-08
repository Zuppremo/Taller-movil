using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using UserAPI.Models;

namespace UserAPI.Context
{
    public class UserDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
        {
        }

    }
}

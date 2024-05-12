using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Context
{
    public class UserDataContext : DbContext
    {
        static readonly string connectionString = "Server=localhost;Port=3306;User ID=root;Password=admin;Database=movildb";

        public DbSet<User> Users { get; set; }
        public DbSet<RequestLogin> LoginRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

    }
}

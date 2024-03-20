using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _2_uzduotis_c_sharp.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<GoogleUser> GoogleUsers { get; set; }
        public DbSet<GitHubUser> GitHubUsers { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

    }
}

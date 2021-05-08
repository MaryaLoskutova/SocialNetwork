using Microsoft.EntityFrameworkCore;
using UsersApi.DataBases;

namespace UsersApi
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserDbo> Users { get; set; }
    }
}
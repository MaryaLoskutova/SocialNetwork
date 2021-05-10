using Microsoft.EntityFrameworkCore;
using UsersApi.BusinessObjects;

namespace UsersApi
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserDbo> Users { get; set; }
        public DbSet<SubscriptionDbo> Subscriptions { get; set; }
    }
}
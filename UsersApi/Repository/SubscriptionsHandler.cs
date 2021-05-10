using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public class SubscriptionsHandler : ISubscriptionsHandler
    {
        private readonly UsersDbContext _context;

        public SubscriptionsHandler(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionDbo> FindAsync(Guid subscriberId, Guid userId)
        {
            return await _context.Subscriptions
                .Where(x =>
                    x.SubscriberId == subscriberId
                    && x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<SubscriptionDbo> CreateAsync(SubscriptionDbo subscription)
        {
            await _context.AddAsync(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task<SubscriptionDbo[]> SelectAsync()
        { 
            return await _context.Subscriptions.ToArrayAsync();
        }
    }
}
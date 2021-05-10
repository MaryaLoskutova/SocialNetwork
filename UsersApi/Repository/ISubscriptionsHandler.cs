using System;
using System.Linq;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public interface ISubscriptionsHandler
    {
        Task<SubscriptionDbo> FindAsync(Guid subscriberId, Guid userId);
        Task<SubscriptionDbo> CreateAsync(SubscriptionDbo subscription);
        Task<SubscriptionDbo[]> SelectAsync();
    }
}
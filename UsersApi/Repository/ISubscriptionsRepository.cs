using System;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public interface ISubscriptionsRepository
    {
        Task<Result<SubscriptionDbo>> SubscribeAsync(Guid subscriberId, Guid userId);
        Task<SubscriptionDbo[]> SelectAsync();
    }
}
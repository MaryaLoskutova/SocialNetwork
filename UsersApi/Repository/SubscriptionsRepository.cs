using System;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;
using UsersApi.Factories;

namespace UsersApi.Repository
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly ISubscriptionsHandler _subscriptionsHandler;
        private readonly ISubscriptionFactory _subscriptionFactory;

        public SubscriptionsRepository(
            ISubscriptionsHandler subscriptionsHandler,
            ISubscriptionFactory subscriptionFactory
        )
        {
            _subscriptionsHandler = subscriptionsHandler;
            _subscriptionFactory = subscriptionFactory;
        }

        public async Task<Result<SubscriptionDbo>> SubscribeAsync(Guid subscriberId, Guid userId)
        {
            if (await _subscriptionsHandler.FindAsync(subscriberId, userId) != null)
            {
                return Result<SubscriptionDbo>.Error("Subscription already exists");
            }

            var subscription = await _subscriptionsHandler.CreateAsync(
                _subscriptionFactory.Create(subscriberId, userId)
                );
            return Result<SubscriptionDbo>.Ok(subscription);
        }

        public Task<SubscriptionDbo[]> SelectAsync()
        {
            return _subscriptionsHandler.SelectAsync();
        }
    }
}
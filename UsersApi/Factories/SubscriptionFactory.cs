using System;
using UsersApi.BusinessObjects;

namespace UsersApi.Factories
{
    public class SubscriptionFactory : ISubscriptionFactory
    {
        public SubscriptionDbo Create(Guid subscriberId, Guid userId)
        {
            return new SubscriptionDbo
            {
                SubscriptionId = Guid.NewGuid(),
                SubscriberId = subscriberId,
                UserId = userId
            };
        }
    }
}
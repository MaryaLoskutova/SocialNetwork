using System;
using UsersApi.BusinessObjects;

namespace UsersApi.Factories
{
    public interface ISubscriptionFactory
    {
        SubscriptionDbo Create(Guid subscriberId, Guid userId);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using UsersApi.BusinessObjects;
using UsersApi.Converters;
using UsersApi.Factories;
using UsersApi.Repository;
using UsersApi.Services;

namespace UsersApi.UnitTests
{
    public class UsersServiceTest
    {
        private IUsersService _usersService;
        private Mock<IUsersRepository> _usersRepository;
        private Mock<ISubscriptionsRepository> _subscriptionsRepository;
        private Mock<IMemoryCache> _memoryCache;
        private Mock<ICacheEntry> _cacheEntry;

        [SetUp]
        public void Setup()
        {
            _usersRepository = new Mock<IUsersRepository>();
            _subscriptionsRepository = new Mock<ISubscriptionsRepository>();
            _memoryCache = new Mock<IMemoryCache>();
            _cacheEntry = new Mock<ICacheEntry>();
            _usersService = new UsersService(
                _usersRepository.Object,
                _subscriptionsRepository.Object,
                new UserConverter(),
                _memoryCache.Object
            );
        }

        [TestCaseSource(nameof(GetRegisterTestData))]
        public async Task RegisterAsyncTest(Result<UserDbo> registerResult, Result<Guid> expected)
        {
            var user = new UserDto {Name = "name"};
            _usersRepository.Setup(x => x.CreateAsync(user)).ReturnsAsync(registerResult);

            var result = await _usersService.RegisterAsync(user);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(nameof(GetSubscribeTestData))]
        public async Task SubscribeAsyncTest(
            Guid subscriberId,
            Guid userId,
            UserDbo[] foundUsers,
            Result<SubscriptionDbo> subscription,
            Result<Guid> expected)
        {
            _usersRepository.Setup(x => x.SelectAsync(new[] {subscriberId, userId})).ReturnsAsync(foundUsers);
            _subscriptionsRepository.Setup(x => x.SubscribeAsync(subscriberId, userId)).ReturnsAsync(subscription);

            var result = await _usersService.SubscribeAsync(subscriberId, userId);
            result.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> GetRegisterTestData()
        {
            var userId = Guid.NewGuid();
            var user = new UserDbo {UserId = userId};

            yield return new TestCaseData(Result<UserDbo>.Error("message"), Result<Guid>.Error("message"))
                .SetName("Register error");
            yield return new TestCaseData(Result<UserDbo>.Ok(user), Result<Guid>.Ok(userId))
                .SetName("Success registration");
        }

        private static IEnumerable<TestCaseData> GetSubscribeTestData()
        {
            var subscriberId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var subscriber = new UserDbo {UserId = subscriberId};
            var user = new UserDbo {UserId = userId};
            var subscription = new SubscriptionDbo() {SubscriptionId = Guid.NewGuid()};
            yield return new TestCaseData(subscriberId, userId, Array.Empty<UserDbo>(),
                    null, Result<Guid>.Error($"Subscriber {subscriberId} not found"))
                .SetName("Subscriber not found");
            yield return new TestCaseData(subscriberId, userId, new[] {subscriber},
                    null, Result<Guid>.Error($"User {userId} not found"))
                .SetName("User not found");
            yield return new TestCaseData(subscriberId, userId, new[] {subscriber, user},
                    Result<SubscriptionDbo>.Error("error"), Result<Guid>.Error("error"))
                .SetName("Subscribe error");
            yield return new TestCaseData(subscriberId, userId, new[] {subscriber, user},
                    Result<SubscriptionDbo>.Ok(subscription), Result<Guid>.Ok(subscription.SubscriptionId))
                .SetName("Subscribe success");
        }

        [TestCaseSource(nameof(GetTopPopularUsersTestData))]
        public async Task SelectTopPopularUsersAsyncTest(
            int count,
            SubscriptionDbo[] selectedSubscriptions,
            Guid[] userIds,
            UserDbo[] selectedUsers,
            UserDto[] expected
        )
        {
            _memoryCache
                .Setup(x => x.CreateEntry("topUsersKey"))
                .Returns(_cacheEntry.Object);
            _cacheEntry.Setup(x => x.Value);

            _subscriptionsRepository.Setup(x => x.SelectAsync()).ReturnsAsync(selectedSubscriptions);
            _usersRepository.Setup(x => x.SelectAsync(userIds)).ReturnsAsync(selectedUsers);

            var result = await _usersService.SelectTopPopularAsync(count);
            result.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> GetTopPopularUsersTestData()
        {
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var user1 = new UserDto {Name = "user1", SubscribersCount = 1};
            var user2 = new UserDto {Name = "user2", SubscribersCount = 2};
            var user3 = new UserDto {Name = "user3", SubscribersCount = 3};
            var userDbo1 = new UserDbo {Name = "user1", UserId = userId1};
            var userDbo2 = new UserDbo {Name = "user2", UserId = userId2};
            var userDbo3 = new UserDbo {Name = "user3", UserId = userId3};
            var subscription11 = new SubscriptionDbo()
                {SubscriberId = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), UserId = userId1};
            var subscription21 = new SubscriptionDbo()
                {SubscriberId = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), UserId = userId2};
            var subscription22 = new SubscriptionDbo()
                {SubscriberId = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), UserId = userId2};
            var subscription31 = new SubscriptionDbo()
                {SubscriberId = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), UserId = userId3};
            var subscription32 = new SubscriptionDbo()
                {SubscriberId = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), UserId = userId3};
            var subscription33 = new SubscriptionDbo()
                {SubscriberId = Guid.NewGuid(), SubscriptionId = Guid.NewGuid(), UserId = userId3};

            yield return new TestCaseData(2, Array.Empty<SubscriptionDbo>(), Array.Empty<Guid>(),
                    Array.Empty<UserDbo>(), Array.Empty<UserDto>())
                .SetName("No subscriptions");
            yield return new TestCaseData(2, new[] {subscription11}, new[] {userId1},
                    new[] {userDbo1}, new[] {user1})
                .SetName("One subscription");
            yield return new TestCaseData(
                    2,
                    new[] {subscription11, subscription21, subscription22, subscription31, subscription32, subscription33},
                    new[] {userId1, userId2, userId3},
                    new[] {userDbo1, userDbo2, userDbo3},
                    new[] {user3, user2})
                .SetName("Several users with subscriptions");
        }
    }
}
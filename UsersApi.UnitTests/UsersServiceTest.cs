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
        private Mock<IUserFactory> _userFactory;

        [SetUp]
        public void Setup()
        {
            _usersRepository = new Mock<IUsersRepository>();
            _subscriptionsRepository = new Mock<ISubscriptionsRepository>();
            _memoryCache = new Mock<IMemoryCache>();
            _cacheEntry = new Mock<ICacheEntry>();
            _userFactory = new Mock<IUserFactory>();

            _usersService = new UsersService(
                _usersRepository.Object,
                _subscriptionsRepository.Object,
                new UserConverter(),
                _memoryCache.Object,
                _userFactory.Object
            );
        }

        [Test]
        public async Task CreateWhenUserExistsTest()
        {
            var name = "Mariia";
            var user = new UserDto() {Name = name};
            var userDbo = new UserDbo() {Name = name, UserId = Guid.NewGuid()};
            var expected = Result<Guid>.Error("User with the same name exists");

            _usersRepository.Setup(p => p.FindAsync(user.Name)).ReturnsAsync(userDbo);

            var result = await _usersService.RegisterAsync(user);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CreateSuccessTest()
        {
            var name = "Mariia";
            var user = new UserDto() {Name = name};
            var userId = Guid.NewGuid();
            var userDbo = new UserDbo() {Name = name, UserId = userId};
            var expected = Result<Guid>.Ok(userId);

            _usersRepository.Setup(p => p.FindAsync(user.Name)).ReturnsAsync((UserDbo) null);
            _userFactory.Setup(p => p.Create(user)).Returns(userDbo);
            _usersRepository.Setup(p => p.CreateAsync(userDbo)).ReturnsAsync(userDbo);

            var result = await _usersService.RegisterAsync(user);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(nameof(GetSubscribeTestData))]
        public async Task SubscribeAsyncTest(
            Guid subscriberId,
            Guid userId,
            UserDbo[] foundUsers,
            UserDbo user,
            Result<SubscriptionDbo> subscription,
            Result<Guid> expected)
        {
            _usersRepository.Setup(x => x.SelectAsync(new[] {subscriberId, userId})).ReturnsAsync(foundUsers);
            _usersRepository.Setup(x => x.UpdateAsync(user));
            _subscriptionsRepository.Setup(x => x.SubscribeAsync(subscriberId, userId)).ReturnsAsync(subscription);

            var result = await _usersService.SubscribeAsync(subscriberId, userId);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(nameof(GetTopPopularUsersTestData))]
        public async Task SelectTopPopularUsersAsyncTest(
            int count,
            SubscriptionDbo[] selectedSubscriptions,
            UserDbo[] selectedUsers,
            UserDto[] expected
        )
        {
            _memoryCache
                .Setup(x => x.CreateEntry("topUsersKey"))
                .Returns(_cacheEntry.Object);
            _cacheEntry.Setup(x => x.Value);

            _subscriptionsRepository.Setup(x => x.SelectAsync()).ReturnsAsync(selectedSubscriptions);
            _usersRepository.Setup(x => x.SelectAsync()).ReturnsAsync(selectedUsers);

            var result = await _usersService.SelectTopPopularAsync(count);
            result.Should().BeEquivalentTo(expected);
        }

        #region Test cases

        private static IEnumerable<TestCaseData> GetSubscribeTestData()
        {
            var subscriberId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var subscriber = new UserDbo {UserId = subscriberId};
            var user = new UserDbo {UserId = userId};
            var subscription = new SubscriptionDbo() {SubscriptionId = Guid.NewGuid()};
            
            yield return new TestCaseData(subscriberId, userId, Array.Empty<UserDbo>(), null,
                    null, Result<Guid>.Error($"Subscriber {subscriberId} not found"))
                .SetName("Subscriber not found");
            yield return new TestCaseData(subscriberId, userId, new[] {subscriber}, null,
                    null, Result<Guid>.Error($"User {userId} not found"))
                .SetName("User not found");
            yield return new TestCaseData(subscriberId, userId, new[] {subscriber, user}, user,
                    Result<SubscriptionDbo>.Error("error"), Result<Guid>.Error("error"))
                .SetName("Subscribe error");
            yield return new TestCaseData(subscriberId, userId, new[] {subscriber, user}, user,
                    Result<SubscriptionDbo>.Ok(subscription), Result<Guid>.Ok(subscription.SubscriptionId))
                .SetName("Subscribe success");
            yield return new TestCaseData(subscriberId, subscriberId, new[] {subscriber}, null,
                    null, Result<Guid>.Error("User can't subscribe on himself"))
                .SetName("Subscribe on himself");
        }


        private static IEnumerable<TestCaseData> GetTopPopularUsersTestData()
        {
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userId3 = Guid.NewGuid();
            var user1Name = "user1";
            var user2Name = "user2";
            var user3Name = "user3";
            var user1 = new UserDto {Name = user1Name, SubscribersCount = 1};
            var user2 = new UserDto {Name = user2Name, SubscribersCount = 2};
            var user3 = new UserDto {Name = user3Name, SubscribersCount = 3};
            var user2WithoutSubscribers = new UserDto {Name = user2Name, SubscribersCount = 0};
            var user3WithoutSubscribers = new UserDto {Name = user3Name, SubscribersCount = 0};
            var userDbo1 = new UserDbo {Name = user1Name, UserId = userId1, Timestamp = DateTime.Now.AddDays(-1).Ticks};
            var userDbo2 = new UserDbo {Name = user2Name, UserId = userId2, Timestamp = DateTime.Now.AddDays(-2).Ticks};
            var userDbo3 = new UserDbo {Name = user3Name, UserId = userId3, Timestamp = DateTime.Now.AddDays(-3).Ticks};
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

            yield return new TestCaseData(2, Array.Empty<SubscriptionDbo>(),
                    Array.Empty<UserDbo>(), Array.Empty<UserDto>())
                .SetName("No subscriptions, no users");
            yield return new TestCaseData(2, Array.Empty<SubscriptionDbo>(),
                    new[] {userDbo2, userDbo3}, new[] {user2WithoutSubscribers, user3WithoutSubscribers})
                .SetName("No subscriptions, users exist");
            yield return new TestCaseData(2,  new[] {subscription11},
                    new[] {userDbo1, userDbo2, userDbo3}, new[] {user1, user2WithoutSubscribers})
                .SetName("One subscription");
            yield return new TestCaseData(
                    2,
                    new[]
                    {
                        subscription11, subscription21, subscription22, subscription31, subscription32, subscription33
                    },
                    new[] {userDbo1, userDbo2, userDbo3},
                    new[] {user3, user2})
                .SetName("Several users with subscriptions");
        }

        #endregion
    }
}
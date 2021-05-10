using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using UsersApi.BusinessObjects;
using UsersApi.Converters;
using UsersApi.Repository;

namespace UsersApi.Services
{
    public class UsersService : IUsersService
    {
        private const string TopUsersKey = "topUsersKey";
        private readonly IUsersRepository _usersRepository;
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IUserConverter _userConverter;
        private readonly IMemoryCache _memoryCache;

        public UsersService(
            IUsersRepository usersRepository,
            ISubscriptionsRepository subscriptionsRepository,
            IUserConverter userConverter,
            IMemoryCache memoryCache
            )
        {
            _usersRepository = usersRepository;
            _subscriptionsRepository = subscriptionsRepository;
            _userConverter = userConverter;
            _memoryCache = memoryCache;
        }

        public async Task<Result<Guid>> RegisterAsync(UserDto user)
        {
            var result = await _usersRepository.CreateAsync(user);
            return result.IsSuccess
                ? Result<Guid>.Ok(result.Value.UserId)
                : Result<Guid>.Error(result.ErrorMessage);
        }

        public async Task<Result<Guid>> SubscribeAsync(Guid subscriberId, Guid userId)
        {
            var users = await _usersRepository.SelectAsync(new[] {subscriberId, userId});
            if (users.All(x => x.UserId != subscriberId))
            {
                return Result<Guid>.Error($"Subscriber {subscriberId} not found");
            }

            if (users.All(x => x.UserId != userId))
            {
                return Result<Guid>.Error($"User {userId} not found");
            }

            var result = await _subscriptionsRepository.SubscribeAsync(subscriberId, userId);
            return result.IsSuccess
                ? Result<Guid>.Ok(result.Value.SubscriptionId)
                : Result<Guid>.Error(result.ErrorMessage);
        }

        public async Task<UserDto[]> SelectTopPopularAsync(int count)
        {
            var users = await _memoryCache.GetOrCreateAsync(TopUsersKey, entry => {
                entry.SlidingExpiration = TimeSpan.FromSeconds(20);
                return SelectUsersAsync();
            });
            return users.Take(count).ToArray();
        }

        private async Task<UserDto[]> SelectUsersAsync()
        {
            var subscriptions = await _subscriptionsRepository.SelectAsync();
            var usersDictionary = subscriptions
                .GroupBy(x => x.UserId)
                .ToDictionary(x => x.Key, y => y.Count());
            var users = await _usersRepository.SelectAsync(usersDictionary.Select(x => x.Key).ToArray());
            return users
                .Select(x => _userConverter.ToDto(x, usersDictionary[x.UserId]))
                .OrderByDescending(x => x.SubscribersCount)
                .ToArray();
        }
    }
}
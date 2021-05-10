using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using UsersApi.BusinessObjects;
using UsersApi.Converters;
using UsersApi.Factories;
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
        private readonly IUserFactory _userFactory;

        public UsersService(
            IUsersRepository usersRepository,
            ISubscriptionsRepository subscriptionsRepository,
            IUserConverter userConverter,
            IMemoryCache memoryCache,
            IUserFactory userFactory
            )
        {
            _usersRepository = usersRepository;
            _subscriptionsRepository = subscriptionsRepository;
            _userConverter = userConverter;
            _memoryCache = memoryCache;
            _userFactory = userFactory;
        }

        public async Task<Result<Guid>> RegisterAsync(UserDto user)
        {
            var existedUser = await _usersRepository.FindAsync(user.Name);
            if (existedUser != null)
            {
                return Result<Guid>.Error("User with the same name exists");
            }
            var result = await _usersRepository.CreateAsync(_userFactory.Create(user));
            return Result<Guid>.Ok(result.UserId);
        }

        public async Task<Result<Guid>> SubscribeAsync(Guid subscriberId, Guid userId)
        {
            if (subscriberId == userId)
            {   
                return Result<Guid>.Error("User can't subscribe on himself");
            }
            
            var users = await _usersRepository.SelectAsync(new[] {subscriberId, userId});
            var sudscriber = users.FirstOrDefault(x => x.UserId == subscriberId);
            if (sudscriber == null)
            {
                return Result<Guid>.Error($"Subscriber {subscriberId} not found");
            }
            var user = users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                return Result<Guid>.Error($"User {userId} not found");
            }

            var result = await _subscriptionsRepository.SubscribeAsync(subscriberId, userId);
            await _usersRepository.UpdateAsync(user);
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
            var users = await _usersRepository.SelectAsync();
            return users
                .Select(x => _userConverter.ToDto(x, usersDictionary.ContainsKey(x.UserId) ? usersDictionary[x.UserId] : 0))
                .OrderByDescending(x => x.SubscribersCount)
                .ToArray();
        }
    }
}
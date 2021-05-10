using System;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Services
{
    public interface IUsersService
    {
        Task<Result<Guid>> RegisterAsync(UserDto user);
        Task<Result<Guid>> SubscribeAsync(Guid subscriberId, Guid userId);
        Task<UserDto[]> SelectTopPopularAsync(int count);
    }
}
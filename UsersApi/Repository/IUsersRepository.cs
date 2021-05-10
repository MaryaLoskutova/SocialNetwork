using System;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public interface IUsersRepository
    {
        Task<Result<UserDbo>> CreateAsync(UserDto user);
        Task<UserDbo[]> SelectAsync(Guid[] userIds);
    }
}
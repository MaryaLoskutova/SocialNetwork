using System;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public interface IUsersRepository
    {
        Task<UserDbo> CreateAsync(UserDbo user);
        Task<UserDbo[]> SelectAsync(params Guid[] userIds);
        Task<UserDbo> UpdateAsync(UserDbo user);
        Task<UserDbo> FindAsync(string userName);
    }
}
using System;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public interface IUsersHandler
    {
        Task<UserDbo> FindAsync(string name);
        Task<UserDbo[]> SelectAsync(Guid[] userIds);
        Task<UserDbo> CreateAsync(UserDbo userDbo);
    }
}
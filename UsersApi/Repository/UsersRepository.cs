using System;
using System.Linq;
using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IUsersHandler _usersHandler;

        public UsersRepository(
            IUsersHandler usersHandler
            )
        {
            _usersHandler = usersHandler;
        }

        public Task<UserDbo> CreateAsync(UserDbo user)
        {
            user.Timestamp = DateTime.Now.Ticks;
            return _usersHandler.CreateAsync(user);
        }

        public Task<UserDbo> UpdateAsync(UserDbo user)
        {
            user.Timestamp = DateTime.Now.Ticks;
            return _usersHandler.UpdateAsync(user);

        }

        public Task<UserDbo> FindAsync(string userName)
        {
            return _usersHandler.FindAsync(userName);
        }

        public Task<UserDbo[]> SelectAsync(params Guid[] userIds)
        {
            if (userIds.Any())
            {
                return _usersHandler.SelectAsync(userIds);
            }

            return _usersHandler.SelectAsync();
        }
    }
}
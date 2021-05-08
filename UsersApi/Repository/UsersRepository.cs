using System.Threading.Tasks;
using UsersApi.BusinessObjects;
using UsersApi.Factories;

namespace UsersApi.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IUsersHandler _usersHandler;
        private readonly IUserFactory _userFactory;

        public UsersRepository(
            IUsersHandler usersHandler,
            IUserFactory userFactory
            )
        {
            _usersHandler = usersHandler;
            _userFactory = userFactory;
        }

        public async Task<Result> CreateAsync(UserDto user)
        {
            if (await _usersHandler.FindAsync(user.Name) != null)
            {
                return Result.Error("The user with the same name exists");
            }
            
            await _usersHandler.CreateAsync(_userFactory.Create(user));
            return Result.Ok();
        }
    }
}
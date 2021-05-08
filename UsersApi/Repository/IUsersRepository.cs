using System.Threading.Tasks;
using UsersApi.BusinessObjects;

namespace UsersApi.Repository
{
    public interface IUsersRepository
    {
        Task<Result> CreateAsync(UserDto user);
    }
}
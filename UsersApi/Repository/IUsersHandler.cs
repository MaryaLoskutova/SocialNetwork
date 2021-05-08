using System.Threading.Tasks;
using UsersApi.DataBases;

namespace UsersApi.Repository
{
    public interface IUsersHandler
    {
        Task<UserDbo> FindAsync(string name);
        Task CreateAsync(UserDbo userDbo);
    }
}
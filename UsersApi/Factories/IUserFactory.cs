using UsersApi.BusinessObjects;
using UsersApi.DataBases;

namespace UsersApi.Factories
{
    public interface IUserFactory
    {
        UserDbo Create(UserDto user);
    }
}
using UsersApi.BusinessObjects;

namespace UsersApi.Factories
{
    public interface IUserFactory
    {
        UserDbo Create(UserDto user);
    }
}
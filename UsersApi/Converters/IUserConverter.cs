using UsersApi.BusinessObjects;

namespace UsersApi.Converters
{
    public interface IUserConverter
    {
        UserDto ToDto(User user);
    }
}
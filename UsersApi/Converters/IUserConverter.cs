using UsersApi.BusinessObjects;

namespace UsersApi.Converters
{
    public interface IUserConverter
    {
        UserDto ToDto(UserRegistrationInfo userRegistrationInfo);
        UserDto ToDto(UserDbo user, int subscribersCount);
    }
}
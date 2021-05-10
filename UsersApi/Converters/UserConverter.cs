using UsersApi.BusinessObjects;

namespace UsersApi.Converters
{
    public class UserConverter : IUserConverter
    {
        public UserDto ToDto(UserRegistrationInfo userRegistrationInfo)
        {
            return new UserDto
            {
                Name = userRegistrationInfo.Name.Trim()
            };
        }

        public UserDto ToDto(UserDbo user, int subscribersCount)
        {
            return new UserDto()
            {
                SubscribersCount = subscribersCount,
                Name = user.Name
            };
        }
    }
}
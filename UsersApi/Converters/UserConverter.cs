using UsersApi.BusinessObjects;

namespace UsersApi.Converters
{
    public class UserConverter : IUserConverter
    {
        public UserDto ToDto(User user)
        {
            return new UserDto
            {
                Name = user.Name.Trim()
            };
        }
    }
}
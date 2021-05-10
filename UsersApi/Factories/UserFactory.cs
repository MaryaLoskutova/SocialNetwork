using System;
using UsersApi.BusinessObjects;

namespace UsersApi.Factories
{
    public class UserFactory : IUserFactory
    {
        public UserDbo Create(UserDto user)
        {
            return new UserDbo
            {
                UserId = Guid.NewGuid(),
                Name = user.Name
            };
        }
    }
}
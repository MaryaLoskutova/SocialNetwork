using System.Text.RegularExpressions;
using UsersApi.BusinessObjects;

namespace UsersApi.Validators
{
    public class UserValidator : IUserValidator
    {
        private static readonly Regex UserNameRegex = new Regex(@"^[\p{L} ]+$");
        public Result<UserRegistrationInfo> Validate(UserRegistrationInfo userRegistrationInfo)
        {
            var userName = userRegistrationInfo.Name.Trim();
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Result<UserRegistrationInfo>.Error("User name should contain letters");
            }
            if (userName.Length > 64)
            {
                return Result<UserRegistrationInfo>.Error("User name length is too long. Max - 64");
            }
            return !UserNameRegex.IsMatch(userName) 
                ? Result<UserRegistrationInfo>.Error("User name should contain only letters and white spaces") 
                : Result<UserRegistrationInfo>.Ok(userRegistrationInfo);
        }
    }
}
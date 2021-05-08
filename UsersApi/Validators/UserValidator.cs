using System.Text.RegularExpressions;
using UsersApi.BusinessObjects;

namespace UsersApi.Validators
{
    public class UserValidator : IUserValidator
    {
        private static readonly Regex UserNameRegex = new Regex(@"^[\p{L} ]+$");
        public Result Validate(User user)
        {
            var userName = user.Name.Trim();
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Result.Error("User name should contain letters");
            }
            if (userName.Length > 64)
            {
                return Result.Error("User name length is too long. Max - 64");
            }
            return !UserNameRegex.IsMatch(userName) 
                ? Result.Error("User name should contain only letters and white spaces") 
                : Result.Ok();
        }
    }
}
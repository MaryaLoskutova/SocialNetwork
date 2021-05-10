using System.Diagnostics.CodeAnalysis;
using UsersApi.BusinessObjects;

namespace UsersApi.Validators
{
    public interface IUserValidator
    {
        Result<UserRegistrationInfo> Validate([NotNull] UserRegistrationInfo userRegistrationInfo);
    }
}
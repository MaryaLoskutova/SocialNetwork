using System.Diagnostics.CodeAnalysis;
using UsersApi.BusinessObjects;

namespace UsersApi.Validators
{
    public interface IUserValidator
    {
        Result Validate([NotNull] User user);
    }
}
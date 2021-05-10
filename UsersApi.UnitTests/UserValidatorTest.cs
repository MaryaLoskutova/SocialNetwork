using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UsersApi.BusinessObjects;
using UsersApi.Validators;

namespace UsersApi.UnitTests
{
    public class UserValidatorTest
    {
        private IUserValidator _userValidator;
        private static string _regexError = "User name should contain only letters and white spaces";
        private static string _nameEmptyError = "User name should contain letters";
        private static string _lengthError = "User name length is too long. Max - 64";

        [SetUp]
        public void Setup()
        {
            _userValidator = new UserValidator();
        }

        [TestCaseSource(nameof(SelectUserTestData))]
        public void ValidateTest(UserRegistrationInfo userRegistrationInfo, Result<UserRegistrationInfo> expected)
        {
            var result = _userValidator.Validate(userRegistrationInfo);
            result.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> SelectUserTestData()
        {
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = string.Empty}, Result<UserRegistrationInfo>.Error(_nameEmptyError))
                .SetName("User name is empty");
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = "     "}, Result<UserRegistrationInfo>.Error(_nameEmptyError))
                .SetName("User name is white space");
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = new String('a', 65)}, Result<UserRegistrationInfo>.Error(_lengthError))
                .SetName("User name is too long");
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = "Ann65"}, Result<UserRegistrationInfo>.Error(_regexError))
                .SetName("User name contain numbers");
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = "Ann_Rose"}, Result<UserRegistrationInfo>.Error(_regexError))
                .SetName("User name contain symbols");
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = "Мария"}, Result<UserRegistrationInfo>.Ok(new UserRegistrationInfo {Name = "Мария"}))
                .SetName("Russian letters");
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = new String('a', 64)}, Result<UserRegistrationInfo>.Ok(new UserRegistrationInfo {Name = new String('a', 64)}))
                .SetName("User name is 64 characters long");
            yield return new TestCaseData(
                    new UserRegistrationInfo {Name = "NameWithoutSpaces"}, Result<UserRegistrationInfo>.Ok(new UserRegistrationInfo {Name = "NameWithoutSpaces"}))
                .SetName("User name without spaces");
        }
    }
}
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
        public void ValidateTest(User user, Result expected)
        {
            var result = _userValidator.Validate(user);
            result.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> SelectUserTestData()
        {
            yield return new TestCaseData(
                    new User {Name = string.Empty}, Result.Error(_nameEmptyError))
                .SetName("User name is empty");
            yield return new TestCaseData(
                    new User {Name = "     "}, Result.Error(_nameEmptyError))
                .SetName("User name is white space");
            yield return new TestCaseData(
                    new User {Name = new String('a', 65)}, Result.Error(_lengthError))
                .SetName("User name is too long");
            yield return new TestCaseData(
                    new User {Name = "Ann65"}, Result.Error(_regexError))
                .SetName("User name contain numbers");
            yield return new TestCaseData(
                    new User {Name = "Ann_Rose"}, Result.Error(_regexError))
                .SetName("User name contain symbols");
            yield return new TestCaseData(
                    new User {Name = "Мария"}, Result.Ok())
                .SetName("Russian letters");
            yield return new TestCaseData(
                    new User {Name = new String('a', 64)}, Result.Ok())
                .SetName("User name is 64 characters long");
            yield return new TestCaseData(
                    new User {Name = "NameWithoutSpaces"}, Result.Ok())
                .SetName("User name without spaces");
        }
    }
}
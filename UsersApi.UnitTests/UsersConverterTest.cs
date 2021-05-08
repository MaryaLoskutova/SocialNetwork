using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UsersApi.BusinessObjects;
using UsersApi.Converters;
using UsersApi.Validators;

namespace UsersApi.UnitTests
{
    public class UsersConverterTest
    {
        private IUserConverter _userConverter;

        [SetUp]
        public void Setup()
        {
            _userConverter = new UserConverter();
        }

        [TestCase("Anne Bolein", "Anne Bolein")]
        [TestCase("       Anne Bolein    ", "Anne Bolein")]
        [TestCase("Anne   Bolein", "Anne   Bolein")]
        public void ValidateTest(string name, string expectedName)
        {
            var user = new User() {Name = name};
            var result = _userConverter.ToDto(user);
            result.Should().BeEquivalentTo(new UserDto {Name = expectedName});
        }
    }
}
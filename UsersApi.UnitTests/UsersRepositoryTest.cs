using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using UsersApi.BusinessObjects;
using UsersApi.Factories;
using UsersApi.Repository;

namespace UsersApi.UnitTests
{
    public class UsersRepositoryTest
    {
        private IUsersRepository _usersRepository;
        private Mock<IUsersHandler> _usersHandler;
        private Mock<IUserFactory> _userFactory;
        
        [SetUp]
        public void Setup()
        {
            _usersHandler = new Mock<IUsersHandler>();
            _userFactory = new Mock<IUserFactory>();
            _usersRepository = new UsersRepository(_usersHandler.Object, _userFactory.Object);
        }

        [Test]
        public async Task CreateWhenUserExistsTest()
        {
            var name = "Mariia";
            var user = new UserDto() {Name = name};
            var userDbo = new UserDbo() {Name = name, UserId = Guid.NewGuid()};
            var expected = Result<UserDbo>.Error("The user with the same name exists");
            
            _usersHandler.Setup(p => p.FindAsync(user.Name)).ReturnsAsync(userDbo);
            
            var result = await _usersRepository.CreateAsync(user);
            result.Should().BeEquivalentTo(expected);
        }
        
        [Test]
        public async Task CreateSuccessTest()
        {
            var name = "Mariia";
            var user = new UserDto() {Name = name};
            var userId = Guid.NewGuid();
            var userDbo = new UserDbo() {Name = name, UserId = userId};
            var expected = Result<UserDbo>.Ok(userDbo);
            
            _usersHandler.Setup(p => p.FindAsync(user.Name)).ReturnsAsync((UserDbo) null);
            _userFactory.Setup(p => p.Create(user)).Returns(userDbo);
            _usersHandler.Setup(p => p.CreateAsync(userDbo)).ReturnsAsync(userDbo);
            
            var result = await _usersRepository.CreateAsync(user);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
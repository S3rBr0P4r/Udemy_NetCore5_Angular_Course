using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Udemy.NetCore5.Angular.Api.Controllers;
using Udemy.NetCore5.Angular.Data;
using Udemy.NetCore5.Angular.Data.Entities;
using Xunit;

namespace Udemy.NetCore5.Angular.Api.Tests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task GivenListOfUsers_WhenEndpointIsCalled_ThenReturnsListOfUsers()
        {
            // Arrange
            var dbSetMock = new List<AppUser> { new AppUser { UserName = "username1" } }.AsQueryable().BuildMockDbSet();
            var dataContextOptions = new DbContextOptionsBuilder<DataContext>().Options;
            var dataContextMock = new Mock<DataContext>(dataContextOptions);
            dataContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            var testee = new UsersController(dataContextMock.Object);

            // Act
            var result = await testee.GetUsers().ConfigureAwait(false);

            // Assert
            result.Count().Should().Be(1, "there is one user in the database");
        }

        [Fact]
        public async Task GivenListOfUsers_WhenUserIsLookedForAndItExists_ThenReturnsTheUser()
        {
            // Arrange
            var dbSetMock = new List<AppUser> { new AppUser { Id = 1, UserName = "username1" } }.AsQueryable().BuildMockDbSet();
            var dataContextOptions = new DbContextOptionsBuilder<DataContext>().Options;
            var dataContextMock = new Mock<DataContext>(dataContextOptions);
            dataContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            var testee = new UsersController(dataContextMock.Object);

            // Act
            var result = await testee.GetUser(1).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull("the user already exists in the database");
            result.UserName.Should().Be("username1");
        }

        [Fact]
        public async Task GivenListOfUsers_WhenUserIsLookedForAndDoesNotExist_ThenReturnsNull()
        {
            // Arrange
            var dbSetMock = new List<AppUser> { new AppUser { Id = 1, UserName = "username1" } }.AsQueryable().BuildMockDbSet();
            var dataContextOptions = new DbContextOptionsBuilder<DataContext>().Options;
            var dataContextMock = new Mock<DataContext>(dataContextOptions);
            dataContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);
            var testee = new UsersController(dataContextMock.Object);

            // Act
            var result = await testee.GetUser(3).ConfigureAwait(false);

            // Assert
            result.Should().BeNull("the user does not exist in the database");
        }
    }
}
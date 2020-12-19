using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Udemy.NetCore5.Angular.Data.Entities;
using Udemy.NetCore5.Angular.Logic.Services;
using Xunit;

namespace Udemy.NetCore5.Angular.Logic.Tests.Services
{
    public class TokenServiceTests
    {
        [Fact]
        public void GivenNonExistingUser_WhenTokenTriesToBeGenerated_ThenReturns_Null()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["TokenKey"]).Returns("Some token for testing");
            var testee = new TokenService(configurationMock.Object);

            // Act
            var result = testee.CreateToken(null);

            // Assert
            result.Should().BeNull("app user is null and therefore a token cannot be generated");
        }

        [Fact]
        public void GivenExistingUser_WhenTokenTriesToBeGenerated_ThenReturns_Token()
        {
            // Arrange
            var appUser = new AppUser {Id = 1, UserName = "UserName"};
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["TokenKey"]).Returns("Some token for testing");
            var testee = new TokenService(configurationMock.Object);

            // Act
            var result = testee.CreateToken(appUser);

            // Assert
            result.Should().NotBeEmpty("app user exists and therefore a token must be generated");
        }

        [Fact]
        public void GivenExistingUser_WhenTokenIsGenerated_ThenTokenContainsExpectedClaims()
        {
            // Arrange
            const string userName = "SomeUser";
            var appUser = new AppUser { Id = 1, UserName = userName };
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c["TokenKey"]).Returns("Some token for testing");
            var testee = new TokenService(configurationMock.Object);

            // Act
            var result = testee.CreateToken(appUser);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(result);
            token.Claims.Single(c => c.Type == JwtRegisteredClaimNames.NameId).Value.Should().Be(userName, $"name id claim with {userName} is the user to generate a token");
            token.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value.Should().NotBeEmpty("expire date time claim must be defined");
            token.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Nbf).Value.Should().NotBeEmpty("never before claim must be defined");
            token.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Iat).Value.Should().NotBeEmpty("issue at claim must be defined");
        }
    }
}
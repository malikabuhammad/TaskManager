using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManager.Application.Auth;
using TaskManager.Domain.Common.Results;
using TaskManager.Domain.Users;
using TaskManager.Infrastructure.Auth;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new AppDbContext(options);

            var hasher = new PasswordHasher();
            var user = User.CreateUser(Guid.NewGuid(), "User", "user@test.com", Role.User).Value;
            user.SetPasswordHash(hasher.Hash("200300123"));
            context.Users.Add(user);
            context.SaveChanges();

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "TestSigningKeyThatIsLongEnoughForHmacSha256!",
                ["Jwt:Issuer"] = "Tests",
                ["Jwt:Audience"] = "Tests"
            }).Build();

            _service = new AuthService(new UserRepository(context), hasher, configuration);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsToken()
        {
            var result = await _service.LoginAsync(new LoginRequest("user@test.com", "200300123"));

            Assert.True(result.IsSuccess);
            Assert.False(string.IsNullOrEmpty(result.Value.Token));
            Assert.Equal("User", result.Value.Role);
        }

        [Fact]
        public async Task LoginAsync_WithWrongPassword_ReturnsUnauthorized()
        {
            var result = await _service.LoginAsync(new LoginRequest("user@test.com", "wrongpassword"));

            Assert.True(result.IsError);
            Assert.Equal(ErrorType.Unauthorized, result.Errors[0].Type);
        }
    }
}

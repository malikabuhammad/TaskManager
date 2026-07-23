using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Common.Results;
using TaskManager.Domain.Users;

namespace TaskManager.Application.Auth
{
    public class AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration) : IAuthService
    {
        public async Task<Result<AuthResponseDto>> LoginAsync(LoginRequest request)
        {
            var user = await userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant());

            if (user is null || !passwordHasher.Verify(user.PasswordHash, request.Password))
            {
                return UserErrors.UserUnauthorized;
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto(token, user.Name, user.Role.ToString());
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

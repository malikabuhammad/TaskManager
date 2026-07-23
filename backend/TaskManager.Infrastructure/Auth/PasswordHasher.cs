using Microsoft.AspNetCore.Identity;

using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Users;

namespace TaskManager.Infrastructure.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<User> _hasher = new();

        public string Hash(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool Verify(string passwordHash, string password)
        {
            var result = _hasher.VerifyHashedPassword(null!, passwordHash, password);

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}

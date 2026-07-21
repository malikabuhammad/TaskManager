using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Domain.Common;
using TaskManager.Domain.Common.Results;

namespace TaskManager.Domain.Users
{
    public class User:Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; } = string.Empty;
        public Role Role { get; private set; }

        private User(Guid id, string name, string email, Role role) : base(id)
        {
            Name = name;
            Email = email;
            Role = role;
        }
        public static Result<User> CreateUser(Guid id, string name, string email, Role role)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                 return UserErrors.UserNameIsRequired;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                return UserErrors.UserEmailIsRequired;
            }
            if (!email.Contains("@"))
            {
                return UserErrors.UserEmailIsInvalid;
            }

            return new User(id, name.Trim(), email.Trim().ToLowerInvariant(), role);

        }

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
        public Result<Success> Update(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return UserErrors.UserNameIsRequired;
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                return UserErrors.UserEmailIsInvalid;
            }

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();

            return Result.Success;
        }
    }
}

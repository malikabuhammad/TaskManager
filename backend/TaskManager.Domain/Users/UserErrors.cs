using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Domain.Common.Results;

namespace TaskManager.Domain.Users
{
    public class UserErrors
    {
        public static Error UserNameIsRequired => Error.Validation("User.NameIsRequired", "User name is required.");
        public static Error UserEmailIsRequired => Error.Validation("User.EmailIsRequired", "User email is required.");
        public static Error UserEmailIsInvalid => Error.Validation("User.EmailIsInvalid", "User email is invalid.");
        public static Error DuplicateEmail => Error.Conflict("User.DuplicateEmail", "A user with this email already exists.");
        public static Error UserPasswordIsRequired => Error.Validation("User.PasswordIsRequired", "User password is required.");
        public static Error UserPasswordIsTooShort => Error.Validation("User.PasswordIsTooShort", "User password is too short.");
        public static Error UserRoleIsRequired => Error.Validation("User.RoleIsRequired", "User role is required.");
        public static Error UserNotFound => Error.NotFound("User.NotFound", "User not found.");
        public static Error UserAlreadyExists => Error.Conflict("User.AlreadyExists", "User already exists.");
        public static Error UserUnauthorized => Error.Unauthorized("User.Unauthorized", "User is unauthorized.");
        public static Error AccessDenied => Error.Forbidden("User.AccessDenied", "You are not allowed to access this user.");
    }
}

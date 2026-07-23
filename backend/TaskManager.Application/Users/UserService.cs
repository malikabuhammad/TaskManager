using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Users.Requests;
using TaskManager.Domain.Common.Results;
using TaskManager.Domain.Users;

namespace TaskManager.Application.Users
{
    public sealed class UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
        : IUserService
    {
        public async Task<Result<UserDto>> CreateAsync(CreateUserRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (await userRepository.ExistsByEmailAsync(email))
            {
                return UserErrors.DuplicateEmail;
            }

            var userResult = User.CreateUser(Guid.NewGuid(), request.Name, email, request.Role);

            if (userResult.IsError)
            {
                return userResult.Errors;
            }

            var user = userResult.Value;
            user.SetPasswordHash(passwordHasher.Hash(request.Password));

            userRepository.Add(user);
            await unitOfWork.SaveChangesAsync();

            return user.ToDto();
        }

        public async Task<Result<UserDto>> GetByIdAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);

            return user is null ? UserErrors.UserNotFound : user.ToDto();
        }

        public async Task<Result<List<UserDto>>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();

            return users.Select(u => u.ToDto()).ToList();
        }

        public async Task<Result<UserDto>> UpdateAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return UserErrors.UserNotFound;
            }

            var email = request.Email.Trim().ToLowerInvariant();

            if (email != user.Email && await userRepository.ExistsByEmailAsync(email))
            {
                return UserErrors.DuplicateEmail;
            }

            var updateResult = user.Update(request.Name, email);

            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            await unitOfWork.SaveChangesAsync();

            return user.ToDto();
        }

        public async Task<Result<Deleted>> DeleteAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return UserErrors.UserNotFound;
            }

            userRepository.Remove(user);
            await unitOfWork.SaveChangesAsync();

            return Result.Deleted;
        }
    }
}

using TaskManager.Application.Users.Requests;
using TaskManager.Domain.Common.Results;

namespace TaskManager.Application.Users
{
    public interface IUserService
    {
        Task<Result<UserDto>> CreateAsync(CreateUserRequest request);

        Task<Result<UserDto>> GetByIdAsync(Guid userId);

        Task<Result<List<UserDto>>> GetAllAsync();

        Task<Result<UserDto>> UpdateAsync(Guid userId, UpdateUserRequest request);

        Task<Result<Deleted>> DeleteAsync(Guid userId);
    }
}

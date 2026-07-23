using TaskManager.Domain.Common.Results;

namespace TaskManager.Application.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> LoginAsync(LoginRequest request);
    }
}

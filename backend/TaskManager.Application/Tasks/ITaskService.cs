using TaskManager.Application.Tasks.Requests;
using TaskManager.Domain.Common.Results;

namespace TaskManager.Application.Tasks
{
    public interface ITaskService
    {
        Task<Result<TaskDto>> CreateAsync(CreateTaskRequest request);

        Task<Result<TaskDto>> GetByIdAsync(Guid taskId);

        Task<Result<List<TaskDto>>> GetAllAsync();

        Task<Result<List<TaskDto>>> GetMyTasksAsync();

        Task<Result<TaskDto>> UpdateAsync(Guid taskId, UpdateTaskRequest request);

        Task<Result<TaskDto>> UpdateStatusAsync(Guid taskId, UpdateTaskStatusRequest request);

        Task<Result<Deleted>> DeleteAsync(Guid taskId);
    }
}

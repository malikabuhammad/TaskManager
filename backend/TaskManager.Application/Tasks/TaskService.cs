using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Requests;
using TaskManager.Domain.Common.Results;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks
{
    public sealed class TaskService(
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
        : ITaskService
    {
        public async Task<Result<TaskDto>> CreateAsync(CreateTaskRequest request)
        {
            if (await userRepository.GetByIdAsync(request.AssignedUserId) is null)
            {
                return TaskErrors.AssignedUserNotFound;
            }

            var taskResult = TaskItem.Create(Guid.NewGuid(), request.Title, request.Description, request.AssignedUserId);

            if (taskResult.IsError)
            {
                return taskResult.Errors;
            }

            var task = taskResult.Value;

            taskRepository.Add(task);
            await unitOfWork.SaveChangesAsync();

            return task.ToDto();
        }

        public async Task<Result<TaskDto>> GetByIdAsync(Guid taskId)
        {
            var task = await taskRepository.GetByIdAsync(taskId);

            if (task is null)
            {
                return TaskErrors.NotFound;
            }

            return task.ToDto();
        }

        public async Task<Result<List<TaskDto>>> GetAllAsync()
        {
            var tasks = await taskRepository.GetAllAsync();

            return tasks.Select(t => t.ToDto()).ToList();
        }

        public async Task<Result<TaskDto>> UpdateAsync(Guid taskId, UpdateTaskRequest request)
        {
            var task = await taskRepository.GetByIdAsync(taskId);

            if (task is null)
            {
                return TaskErrors.NotFound;
            }

            if (await userRepository.GetByIdAsync(request.AssignedUserId) is null)
            {
                return TaskErrors.AssignedUserNotFound;
            }

            var updateResult = task.Update(request.Title, request.Description, request.Status, request.AssignedUserId);

            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            await unitOfWork.SaveChangesAsync();

            return task.ToDto();
        }

        public async Task<Result<TaskDto>> UpdateStatusAsync(Guid taskId, UpdateTaskStatusRequest request)
        {
            var task = await taskRepository.GetByIdAsync(taskId);

            if (task is null)
            {
                return TaskErrors.NotFound;
            }

            var updateResult = task.UpdateStatus(request.Status);

            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            await unitOfWork.SaveChangesAsync();

            return task.ToDto();
        }

        public async Task<Result<Deleted>> DeleteAsync(Guid taskId)
        {
            var task = await taskRepository.GetByIdAsync(taskId);

            if (task is null)
            {
                return TaskErrors.NotFound;
            }

            taskRepository.Remove(task);
            await unitOfWork.SaveChangesAsync();

            return Result.Deleted;
        }
    }
}

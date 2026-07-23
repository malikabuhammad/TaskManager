using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks.Requests
{
    public sealed record UpdateTaskStatusRequest(TaskItemStatus Status);
}

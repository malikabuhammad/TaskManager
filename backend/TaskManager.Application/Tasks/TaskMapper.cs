using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(this TaskItem task)
            => new(task.Id, task.Title, task.Description, task.Status.ToString(), task.AssignedUserId);
    }
}

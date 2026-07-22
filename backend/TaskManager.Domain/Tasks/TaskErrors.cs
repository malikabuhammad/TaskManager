using TaskManager.Domain.Common.Results;

namespace TaskManager.Domain.Tasks;

public static class TaskErrors
{
    public static Error TitleRequired => Error.Validation("Task.TitleRequired", "Task title is required.");

    public static Error NotFound => Error.NotFound("Task.NotFound", "Task does not exist.");

    public static Error AccessDenied => Error.Forbidden("Task.AccessDenied", "You are not allowed to access this task.");

    public static Error AssignedUserNotFound => Error.Validation("Task.AssignedUserNotFound", "The assigned user does not exist.");
}

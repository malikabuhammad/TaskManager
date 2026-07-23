namespace TaskManager.Application.Tasks
{
    public sealed record TaskDto(
        Guid Id,
        string Title,
        string? Description,
        string Status,
        Guid AssignedUserId);
}

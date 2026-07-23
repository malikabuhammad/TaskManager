using System.ComponentModel.DataAnnotations;

using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Tasks.Requests
{
    public sealed record UpdateTaskRequest(
        [Required, MaxLength(200)] string Title,
        [MaxLength(2000)] string? Description,
        TaskItemStatus Status,
        [Required] Guid AssignedUserId);
}

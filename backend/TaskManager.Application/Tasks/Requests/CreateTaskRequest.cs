using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.Tasks.Requests
{
    public sealed record CreateTaskRequest(
        [Required, MaxLength(200)] string Title,
        [MaxLength(2000)] string? Description,
        [Required] Guid AssignedUserId);
}

using System.ComponentModel.DataAnnotations;

using TaskManager.Domain.Users;

namespace TaskManager.Application.Users.Requests
{
    public sealed record CreateUserRequest([Required, MaxLength(100)] string Name,[Required, EmailAddress] string Email,
                                           [Required, MinLength(8)] string Password,[Required] Role Role);
}

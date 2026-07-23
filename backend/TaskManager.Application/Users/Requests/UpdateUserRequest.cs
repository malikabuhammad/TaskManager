using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.Users.Requests
{
    public sealed record UpdateUserRequest([Required, MaxLength(100)] string Name,[Required, EmailAddress] string Email);
}

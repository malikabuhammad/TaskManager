using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.Auth
{
    public sealed record LoginRequest([Required, EmailAddress] string Email, [Required] string Password);
}

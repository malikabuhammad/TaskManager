using System.Security.Claims;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Users;

namespace TaskManager.API.Services
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        public Guid? UserId => Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue("uid"), out var id) ? id : null;

        public bool IsAdmin => httpContextAccessor.HttpContext?.User.IsInRole(nameof(Role.Admin)) ?? false;
    }
}

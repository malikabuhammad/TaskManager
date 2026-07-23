using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Auth;

namespace TaskManager.API.Controllers
{
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ApiController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await authService.LoginAsync(request);

            return result.Match(Ok, Problem);
        }
    }
}

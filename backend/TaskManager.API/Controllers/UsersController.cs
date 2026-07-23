using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Application.Users;
using TaskManager.Application.Users.Requests;

namespace TaskManager.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController(IUserService userService) : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var result = await userService.CreateAsync(request);

            return result.Match(
                user => CreatedAtAction(nameof(GetById), new { userId = user.Id }, user),
                Problem);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var result = await userService.GetByIdAsync(userId);

            return result.Match(Ok, Problem);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await userService.GetAllAsync();

            return result.Match(Ok, Problem);
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> Update(Guid userId, UpdateUserRequest request)
        {
            var result = await userService.UpdateAsync(userId, request);

            return result.Match(Ok, Problem);
        }

        [HttpDelete("{userId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await userService.DeleteAsync(userId);

            return result.Match(_ => NoContent(), Problem);
        }
    }
}

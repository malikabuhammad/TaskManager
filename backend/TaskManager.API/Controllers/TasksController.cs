using Microsoft.AspNetCore.Mvc;

using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Requests;

namespace TaskManager.API.Controllers
{
    [Route("api/tasks")]
    public class TasksController(ITaskService taskService) : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskRequest request)
        {
            var result = await taskService.CreateAsync(request);

            return result.Match(
                task => CreatedAtAction(nameof(GetById), new { taskId = task.Id }, task),Problem);
        }

        [HttpGet("{taskId:guid}")]
        public async Task<IActionResult> GetById(Guid taskId)
        {
            var result = await taskService.GetByIdAsync(taskId);

            return result.Match(Ok, Problem);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await taskService.GetAllAsync();

            return result.Match(Ok, Problem);
        }

        [HttpPut("{taskId:guid}")]
        public async Task<IActionResult> Update(Guid taskId, UpdateTaskRequest request)
        {
            var result = await taskService.UpdateAsync(taskId, request);

            return result.Match(Ok, Problem);
        }

        [HttpPut("{taskId:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid taskId, UpdateTaskStatusRequest request)
        {
            var result = await taskService.UpdateStatusAsync(taskId, request);

            return result.Match(Ok, Problem);
        }

        [HttpDelete("{taskId:guid}")]
        public async Task<IActionResult> Delete(Guid taskId)
        {
            var result = await taskService.DeleteAsync(taskId);

            return result.Match(_ => NoContent(), Problem);
        }
    }
}

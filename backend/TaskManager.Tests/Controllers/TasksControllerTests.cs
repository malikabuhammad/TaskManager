using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManager.API.Controllers;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Requests;
using TaskManager.Domain.Tasks;

namespace TaskManager.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _taskService = new();
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _controller = new TasksController(_taskService.Object);
        }

        [Fact]
        public async Task GetById_WhenTaskExists_ReturnsOk()
        {
            var dto = new TaskDto(Guid.NewGuid(), "Task", null, "Pending", Guid.NewGuid());
            _taskService.Setup(s => s.GetByIdAsync(dto.Id)).ReturnsAsync(dto);

            var response = await _controller.GetById(dto.Id);

            var ok = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetById_WhenTaskDoesNotExist_Returns404()
        {
            _taskService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(TaskErrors.NotFound);

            var response = await _controller.GetById(Guid.NewGuid());

            var problem = Assert.IsType<ObjectResult>(response);
            Assert.Equal(404, problem.StatusCode);
        }

        [Fact]
        public async Task Create_WithValidRequest_ReturnsCreated()
        {
            var request = new CreateTaskRequest("Task", null, Guid.NewGuid());
            var dto = new TaskDto(Guid.NewGuid(), "Task", null, "Pending", request.AssignedUserId);
            _taskService.Setup(s => s.CreateAsync(request)).ReturnsAsync(dto);

            var response = await _controller.Create(request);

            var created = Assert.IsType<CreatedAtActionResult>(response);
            Assert.Equal(dto, created.Value);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Requests;
using TaskManager.Domain.Common.Results;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.Users;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _context;
        private readonly FakeCurrentUser _currentUser = new();
        private readonly TaskService _service;
        private readonly User _admin;
        private readonly User _user;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _context = new AppDbContext(options);

            _admin = User.CreateUser(Guid.NewGuid(), "Admin", "admin@test.com", Role.Admin).Value;
            _user = User.CreateUser(Guid.NewGuid(), "User", "user@test.com", Role.User).Value;
            _context.Users.AddRange(_admin, _user);
            _context.SaveChanges();

            _service = new TaskService(new TaskRepository(_context), new UserRepository(_context), new UnitOfWork(_context), _currentUser);
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_CreatesPendingTask()
        {
            var result = await _service.CreateAsync(new CreateTaskRequest("Write docs", "for the api", _user.Id));

            Assert.True(result.IsSuccess);
            Assert.Equal("Pending", result.Value.Status);
            Assert.Equal(_user.Id, result.Value.AssignedUserId);
        }

        [Fact]
        public async Task CreateAsync_WhenAssignedUserDoesNotExist_Fails()
        {
            var result = await _service.CreateAsync(new CreateTaskRequest("Write docs", null, Guid.NewGuid()));

            Assert.True(result.IsError);
            Assert.Equal(TaskErrors.AssignedUserNotFound, result.Errors[0]);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenUserIsAssigned_UpdatesStatus()
        {
            var task = SeedTask(_user.Id);
            _currentUser.UserId = _user.Id;

            var result = await _service.UpdateStatusAsync(task.Id, new UpdateTaskStatusRequest(TaskItemStatus.InProgress));

            Assert.True(result.IsSuccess);
            Assert.Equal("InProgress", result.Value.Status);
        }

        [Fact]
        public async Task UpdateStatusAsync_WhenUserIsNotAssigned_ReturnsForbidden()
        {
            var task = SeedTask(_admin.Id);
            _currentUser.UserId = _user.Id;

            var result = await _service.UpdateStatusAsync(task.Id, new UpdateTaskStatusRequest(TaskItemStatus.Completed));

            Assert.True(result.IsError);
            Assert.Equal(ErrorType.Forbidden, result.Errors[0].Type);
        }

        [Fact]
        public async Task GetMyTasksAsync_ReturnsOnlyTasksAssignedToCurrentUser()
        {
            SeedTask(_user.Id);
            SeedTask(_user.Id);
            SeedTask(_admin.Id);
            _currentUser.UserId = _user.Id;

            var result = await _service.GetMyTasksAsync();

            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
        }

        private TaskItem SeedTask(Guid assignedUserId)
        {
            var task = TaskItem.Create(Guid.NewGuid(), "Seeded task", null, assignedUserId).Value;
            _context.Tasks.Add(task);
            _context.SaveChanges();

            return task;
        }
    }
}

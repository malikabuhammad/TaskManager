using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Tasks;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public sealed class TaskRepository(AppDbContext context) : ITaskRepository
    {
        public Task<TaskItem?> GetByIdAsync(Guid id)
            => context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        public Task<List<TaskItem>> GetAllAsync()
            => context.Tasks.AsNoTracking().ToListAsync();

        public void Add(TaskItem task)
            => context.Tasks.Add(task);

        public void Remove(TaskItem task)
            => context.Tasks.Remove(task);
    }
}

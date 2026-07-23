using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Common.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);

        Task<List<TaskItem>> GetAllAsync();

        void Add(TaskItem task);

        void Remove(TaskItem task);
    }
}

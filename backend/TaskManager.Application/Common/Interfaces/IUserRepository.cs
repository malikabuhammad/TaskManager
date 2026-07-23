using TaskManager.Domain.Users;

namespace TaskManager.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<List<User>> GetAllAsync();

        Task<bool> ExistsByEmailAsync(string email);

        void Add(User user);

        void Remove(User user);
    }
}

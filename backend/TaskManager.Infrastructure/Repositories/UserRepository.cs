using Microsoft.EntityFrameworkCore;

using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Users;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public sealed class UserRepository(AppDbContext context) : IUserRepository
    {
        public Task<User?> GetByIdAsync(Guid id) => context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public Task<User?> GetByEmailAsync(string email) => context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public Task<List<User>> GetAllAsync() => context.Users.AsNoTracking().ToListAsync();

        public Task<bool> ExistsByEmailAsync(string email) => context.Users.AnyAsync(u => u.Email == email);

        public void Add(User user)=> context.Users.Add(user);

        public void Remove(User user)=> context.Users.Remove(user);
    }
}

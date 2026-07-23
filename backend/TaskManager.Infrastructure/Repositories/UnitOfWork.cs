using TaskManager.Application.Common.Interfaces;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public Task SaveChangesAsync()
            => context.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Tasks;
using TaskManager.Domain.Users;

namespace TaskManager.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            if (await context.Users.AnyAsync())
            {
                return;
            }

            var admin = User.CreateUser(Guid.NewGuid(), "MalikAdmin", "admin@gmail.com", Role.Admin).Value;
            admin.SetPasswordHash(passwordHasher.Hash("200300123"));

            var user = User.CreateUser(Guid.NewGuid(), "MalikUser", "user@gmail.com", Role.User).Value;
            user.SetPasswordHash(passwordHasher.Hash("200300123"));

            context.Users.AddRange(admin, user);

            context.Tasks.AddRange(
                TaskItem.Create(Guid.NewGuid(), "Prepare Api Documentation", "Create a comprehensive API documentation for the task managemnt system", user.Id).Value,
                TaskItem.Create(Guid.NewGuid(), "Fix login bug", "Users report intermittent 401 responses.", user.Id).Value,
                TaskItem.Create(Guid.NewGuid(), "Review access policies", "Audit role-based access rules.", admin.Id).Value);

            await context.SaveChangesAsync();
        }
    }
}

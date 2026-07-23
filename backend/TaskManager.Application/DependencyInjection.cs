using Microsoft.Extensions.DependencyInjection;

using TaskManager.Application.Tasks;
using TaskManager.Application.Users;

namespace TaskManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}

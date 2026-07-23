using TaskManager.Domain.Users;

namespace TaskManager.Application.Users
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User user)=> new(user.Id, user.Name, user.Email, user.Role.ToString());
    }
}

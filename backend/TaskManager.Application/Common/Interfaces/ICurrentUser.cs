namespace TaskManager.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }

        bool IsAdmin { get; }
    }
}

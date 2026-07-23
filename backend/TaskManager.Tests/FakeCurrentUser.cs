using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Tests
{
    public class FakeCurrentUser : ICurrentUser
    {
        public Guid? UserId { get; set; }

        public bool IsAdmin { get; set; }
    }
}

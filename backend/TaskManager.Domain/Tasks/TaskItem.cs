using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Domain.Common;
using TaskManager.Domain.Common.Results;
using TaskManager.Domain.Users;

namespace TaskManager.Domain.Tasks
{
    public class TaskItem:Entity
    {
        public string Title { get; private set; }

        public string? Description { get; private set; }

        public TaskItemStatus Status { get; private set; }

        public Guid AssignedUserId { get; private set; }

        public User? AssignedUser { get; private set; }

        private TaskItem(Guid id, string title, string? description, TaskItemStatus status, Guid assignedUserId): base(id)
        {
            Title = title;
            Description = description;
            Status = status;
            AssignedUserId = assignedUserId;
        }

        public static Result<TaskItem> Create(Guid id, string title, string? description, Guid assignedUserId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return TaskErrors.TitleRequired;
            }

            return new TaskItem(id, title.Trim(), description?.Trim(), TaskItemStatus.Pending, assignedUserId);
        }

        public Result<Success> Update(string title, string? description, TaskItemStatus status, Guid assignedUserId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return TaskErrors.TitleRequired;
            }

            Title = title.Trim();
            Description = description?.Trim();
            Status = status;
            AssignedUserId = assignedUserId;

            return Result.Success;
        }

        public Result<Success> UpdateStatus(TaskItemStatus status)
        {
            Status = status;

            return Result.Success;
        }
    }
}

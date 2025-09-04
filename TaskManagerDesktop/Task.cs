using System;

namespace TaskManagerDesktop
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskCategory Category { get; set; }
        public TaskPriority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public Task()
        {
            Id = 0;
            Title = string.Empty;
            Description = string.Empty;
            Category = TaskCategory.Inne;
            Priority = TaskPriority.Sredni;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
            CompletedDate = null;
        }

        public Task(string title, string description, TaskCategory category, TaskPriority priority)
        {
            Id = 0;
            Title = title ?? string.Empty;
            Description = description ?? string.Empty;
            Category = category;
            Priority = priority;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
            CompletedDate = null;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletedDate = DateTime.Now;
        }

        public void MarkAsIncomplete()
        {
            IsCompleted = false;
            CompletedDate = null;
        }

        public override string ToString()
        {
            return $"{Title} ({Category}) - {(IsCompleted ? "Ukończone" : "Do zrobienia")}";
        }
    }

    public enum TaskCategory
    {
        Praca,
        Dom,
        Zakupy,
        Inne
    }

    public enum TaskPriority
    {
        Niski,
        Sredni,
        Wysoki,
        Krytyczny
    }
}
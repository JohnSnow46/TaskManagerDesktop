using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerDesktop
{
    public class TaskManager
    {
        private List<Task> tasks;
        private int nextId;

        public TaskManager()
        {
            tasks = new List<Task>();
            nextId = 1;
        }

        public event EventHandler TasksChanged;

        public List<Task> Tasks => tasks.ToList();
        public int TaskCount => tasks.Count;
        public int CompletedTaskCount => tasks.Count(t => t.IsCompleted);
        public int PendingTaskCount => tasks.Count(t => !t.IsCompleted);

        public Task AddTask(string title, string description, TaskCategory category, TaskPriority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            var task = new Task(title.Trim(), description?.Trim() ?? "", category, priority)
            {
                Id = nextId++
            };

            tasks.Add(task);
            OnTasksChanged();
            return task;
        }

        public bool EditTask(int id, string title, string description, TaskCategory category, TaskPriority priority)
        {
            var task = GetTaskById(id);
            if (task == null)
                return false;

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            task.Title = title.Trim();
            task.Description = description?.Trim() ?? "";
            task.Category = category;
            task.Priority = priority;

            OnTasksChanged();
            return true;
        }

        public bool RemoveTask(int id)
        {
            var task = GetTaskById(id);
            if (task == null)
                return false;

            tasks.Remove(task);
            OnTasksChanged();
            return true;
        }

        public bool ToggleTaskCompletion(int id)
        {
            var task = GetTaskById(id);
            if (task == null)
                return false;

            if (task.IsCompleted)
                task.MarkAsIncomplete();
            else
                task.MarkAsCompleted();

            OnTasksChanged();
            return true;
        }

        public Task GetTaskById(int id)
        {
            return tasks.FirstOrDefault(t => t.Id == id);
        }

        public List<Task> GetTasksByCategory(TaskCategory category)
        {
            return tasks.Where(t => t.Category == category).ToList();
        }

        public List<Task> GetTasksByPriority(TaskPriority priority)
        {
            return tasks.Where(t => t.Priority == priority).ToList();
        }

        public List<Task> GetCompletedTasks()
        {
            return tasks.Where(t => t.IsCompleted).ToList();
        }

        public List<Task> GetPendingTasks()
        {
            return tasks.Where(t => !t.IsCompleted).ToList();
        }

        public List<Task> GetTasksSortedByPriority()
        {
            return tasks.OrderByDescending(t => (int)t.Priority).ToList();
        }

        public List<Task> GetTasksSortedByDate()
        {
            return tasks.OrderBy(t => t.CreatedDate).ToList();
        }

        public void ClearAllTasks()
        {
            tasks.Clear();
            nextId = 1;
            OnTasksChanged();
        }

        public int ClearCompletedTasks()
        {
            int removedCount = tasks.RemoveAll(t => t.IsCompleted);
            if (removedCount > 0)
                OnTasksChanged();
            return removedCount;
        }

        protected virtual void OnTasksChanged()
        {
            TasksChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void SetNextId(int id)
        {
            nextId = Math.Max(nextId, id + 1);
        }

        internal void LoadTasksFromFile(List<Task> tasksToLoad)
        {
            tasks.Clear();
            tasks.AddRange(tasksToLoad);
            if (tasks.Count > 0)
            {
                int maxId = tasks.Max(t => t.Id);
                SetNextId(maxId);
            }
        }
    }
}
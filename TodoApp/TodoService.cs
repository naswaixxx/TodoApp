using System;
using System.Collections.Generic;
using System.Linq;
using TodoListApp.Models;

namespace TodoListApp.Services
{
    public class TodoService
    {
        private readonly List<TodoModel> _tasks;
        private readonly ITaskRepository _repository;

        public IReadOnlyList<TodoModel> Tasks => _tasks.AsReadOnly();

        public TodoService(ITaskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _tasks = new List<TodoModel>();
        }

        public void AddTask(TodoModel task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _tasks.Add(task);
        }

        public bool UpdateTask(Guid id, string title, string description, DateTime dueDate, TaskPriority priority, TodoListApp.Models.TaskStatus status)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null) return false;

            existing.Title = title;
            existing.Description = description;
            existing.DueDate = dueDate;
            existing.Priority = priority;
            existing.Status = status;

            return true;
        }

        public bool RemoveTask(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                return _tasks.Remove(task);
            }
            return false;
        }

        public bool ChangeStatus(Guid id, TodoListApp.Models.TaskStatus newStatus)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.Status = newStatus;
                return true;
            }
            return false;
        }

        public void SaveToFile(string filePath)
        {
            _repository.Save(filePath, _tasks);
        }

        public void LoadFromFile(string filePath)
        {
            var loadedTasks = _repository.Load(filePath);
            _tasks.Clear();
            _tasks.AddRange(loadedTasks);
        }
    }
}
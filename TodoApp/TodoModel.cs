using System;

namespace TodoListApp.Models
{
    public enum TaskPriority
    {
        Низкий,
        Средний,
        Высокий
    }

    public enum TaskStatus
    {
        Не_начата,
        В_процессе,
        Выполнена
    }

    public class TodoModel
    {
        private string _title;
        private string _description;

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название задачи не может быть пустым.");
                _title = value.Trim();
            }
        }

        public string Description
        {
            get => _description;
            set => _description = value?.Trim() ?? string.Empty;
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
        public TaskPriority Priority { get; set; } = TaskPriority.Средний;
        public TaskStatus Status { get; set; } = TaskStatus.Не_начата;

        public TodoModel() { }

        public TodoModel(string title, string description, DateTime dueDate, TaskPriority priority, TaskStatus status = TaskStatus.Не_начата)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = status;
            CreatedAt = DateTime.Now;
        }
    }
}
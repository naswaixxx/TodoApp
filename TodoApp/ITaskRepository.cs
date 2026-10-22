using System.Collections.Generic;
using TodoListApp.Models;

namespace TodoListApp.Services
{
    public interface ITaskRepository
    {
        List<TodoModel> Load(string filePath);
        void Save(string filePath, List<TodoModel> tasks);
    }
}
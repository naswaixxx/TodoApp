using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TodoListApp.Models;

namespace TodoListApp.Services
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public List<TodoModel> Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<TodoModel>();
            }

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<TodoModel>>(json, _options) ?? new List<TodoModel>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при чтении файла JSON: {ex.Message}", ex);
            }
        }

        public void Save(string filePath, List<TodoModel> tasks)
        {
            try
            {
                string json = JsonSerializer.Serialize(tasks, _options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при сохранении JSON: {ex.Message}", ex);
            }
        }
    }
}
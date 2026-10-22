using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using TodoListApp.Models;
using TodoListApp.Services;

namespace TodoListApp
{
    public partial class MainWindow : Window
    {
        private readonly TodoService _todoService;
        private TodoModel _selectedTask;

        public MainWindow()
        {
            InitializeComponent();

            ITaskRepository repository = new JsonTaskRepository();
            _todoService = new TodoService(repository);

            InitEnumComboBoxes();
            DpDueDate.SelectedDate = DateTime.Now.AddDays(1);
            RefreshList();
        }

        private void InitEnumComboBoxes()
        {
            CmbPriority.ItemsSource = Enum.GetValues(typeof(TaskPriority));
            CmbPriority.SelectedIndex = (int)TaskPriority.Средний;

            CmbStatus.ItemsSource = Enum.GetValues(typeof(TodoListApp.Models.TaskStatus));
            CmbStatus.SelectedIndex = (int)TodoListApp.Models.TaskStatus.Не_начата;
        }

        private void RefreshList()
        {
            LvTasks.ItemsSource = null;
            LvTasks.ItemsSource = _todoService.Tasks;
        }

        private void ClearForm()
        {
            _selectedTask = null;
            TxtTitle.Text = string.Empty;
            TxtDescription.Text = string.Empty;
            DpDueDate.SelectedDate = DateTime.Now.AddDays(1);
            CmbPriority.SelectedIndex = (int)TaskPriority.Средний;
            CmbStatus.SelectedIndex = (int)TodoListApp.Models.TaskStatus.Не_начата;
            LvTasks.UnselectAll();
        }

        private void BtnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtTitle.Text))
                {
                    MessageBox.Show("Введите название задачи!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string title = TxtTitle.Text;
                string description = TxtDescription.Text;
                DateTime dueDate = DpDueDate.SelectedDate ?? DateTime.Now.AddDays(1);
                TaskPriority priority = (TaskPriority)CmbPriority.SelectedItem;
                TodoListApp.Models.TaskStatus status = (TodoListApp.Models.TaskStatus)CmbStatus.SelectedItem;

                if (_selectedTask == null)
                {
                    var newTask = new TodoModel(title, description, dueDate, priority, status);
                    _todoService.AddTask(newTask);
                }
                else
                {
                    _todoService.UpdateTask(_selectedTask.Id, title, description, dueDate, priority, status);
                }

                RefreshList();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LvTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LvTasks.SelectedItem is TodoModel selected)
            {
                _selectedTask = selected;
                TxtTitle.Text = selected.Title;
                TxtDescription.Text = selected.Description;
                DpDueDate.SelectedDate = selected.DueDate;
                CmbPriority.SelectedItem = selected.Priority;
                CmbStatus.SelectedItem = selected.Status;
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTask != null)
            {
                _todoService.RemoveTask(_selectedTask.Id);
                RefreshList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Выберите задачу из списка для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void BtnSaveJson_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                FileName = "tasks.json"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _todoService.SaveToFile(dialog.FileName);
                    MessageBox.Show("Задачи успешно сохранены в JSON!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось сохранить: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnLoadJson_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _todoService.LoadFromFile(dialog.FileName);
                    RefreshList();
                    ClearForm();
                    MessageBox.Show("Задачи успешно загружены из JSON!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось загрузить: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
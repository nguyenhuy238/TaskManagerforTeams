using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {

        private readonly TaskManagerDb1Context _context;
        private readonly int _projectId;
        public event Action TaskCreated;
        public CreateTaskWindow(int projectId)
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _projectId = projectId;
            LoadUsers();
            InitializeComboBoxes();
        }

        private void LoadUsers()
        {
            var users = _context.Users.Where(us => us.Role.Equals("TeamMember")).ToList();
            cmbAssignedTo.ItemsSource = users;
            //cmbAssignedTo.Items.Insert(0, null); // Cho phép không chọn người được gán
        }

        private void InitializeComboBoxes()
        {
            cmbStatus.SelectedIndex = 0; // Mặc định là ToDo
            cmbPriority.SelectedIndex = 1; // Mặc định là Medium
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Task Title is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Status is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cmbPriority.SelectedItem == null)
            {
                MessageBox.Show("Priority is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Tạo Task mới
                var newTask = new Models.Task
                {
                    ProjectId = _projectId,
                    Title = txtTitle.Text,
                    Description = txtDescription.Text,
                    Status = ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString(),
                    Priority = ((ComboBoxItem)cmbPriority.SelectedItem).Content.ToString(),
                    AssignedTo = cmbAssignedTo.SelectedItem != null ? (cmbAssignedTo.SelectedItem as User)?.UserId : null,
                    CreatedBy = GetCurrentUserId(),
                    CreatedAt = DateTime.Now,
                    DueDate = dpDueDate.SelectedDate.HasValue ? DateOnly.FromDateTime(dpDueDate.SelectedDate.Value) : null
                };

                _context.Tasks.Add(newTask);
                _context.SaveChanges();

                TaskCreated?.Invoke();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;
            return currentUser.UserId;
        }
    }
}

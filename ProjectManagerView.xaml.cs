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
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for ProjectManagerView.xaml
    /// </summary>
    public partial class ProjectManagerView : UserControl
    {
        private readonly TaskManagerDb1Context _context;

        public ProjectManagerView()
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            LoadProjects();
            LoadProgressReport();
        }

        private void LoadProjects()
        {
            int currentUserId = GetCurrentUserId();
            var projects = _context.ProjectUserRoles
                .Where(pur => pur.UserId == currentUserId && pur.Role == "ProjectManager")
                .Join(_context.Projects,
                    pur => pur.ProjectId,
                    p => p.ProjectId,
                    (pur, p) => new
                    {
                        p.ProjectId,
                        p.Name,
                        p.Description,
                        p.StartDate,
                        EndDate = p.EndDate ?? null,
                        Progress = _context.Tasks
                            .Where(t => t.ProjectId == p.ProjectId)
                            .Count() > 0
                            ? (double)_context.Tasks
                                .Count(t => t.ProjectId == p.ProjectId && t.Status == "Done")
                                / _context.Tasks.Count(t => t.ProjectId == p.ProjectId) * 100
                            : 0
                    })
                .ToList();

            ProjectsDataGrid.ItemsSource = projects;
        }

        private void ViewTasks_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var project = button?.DataContext as dynamic;
            if (project != null)
            {
                var taskViewWindow = new TaskViewWindow(project.ProjectId);
                taskViewWindow.Owner = Window.GetWindow(this);
                taskViewWindow.TasksUpdated += () =>
                {
                    LoadProjects(); // Làm mới danh sách dự án và tiến độ
                    LoadProgressReport(); // Làm mới báo cáo tổng quát
                };
                taskViewWindow.ShowDialog();
            }
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var project = button?.DataContext as dynamic;
            if (project != null)
            {
                // Mở cửa sổ chỉnh sửa dự án
                var editProjectWindow = new CreateProjectWindow(project.ProjectId);
                editProjectWindow.Owner = Window.GetWindow(this);
                editProjectWindow.ProjectCreated += (name, description, startDate, endDate) =>
                {
                    try
                    {
                        var dbProject = _context.Projects.Find(project.ProjectId);
                        if (dbProject != null)
                        {
                            dbProject.Name = name;
                            dbProject.Description = description;
                            dbProject.StartDate = DateOnly.FromDateTime(startDate);
                            // Fix for CS0173: Explicitly cast null to DateOnly? to resolve type ambiguity in the conditional expression.
                            dbProject.EndDate = endDate.HasValue ? DateOnly.FromDateTime(endDate.Value) : (DateOnly?)null;
                            
                            _context.SaveChanges();
                            LoadProjects();
                            MessageBox.Show("Project updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };
                editProjectWindow.ShowDialog();
            }
        }

        private void LoadProgressReport()
        {
            int currentUserId = GetCurrentUserId();
            var managedProjectIds = _context.ProjectUserRoles
                .Where(pur => pur.UserId == currentUserId && pur.Role == "ProjectManager")
                .Select(pur => pur.ProjectId)
                .ToList();

            var totalTasks = _context.Tasks
                .Where(t => managedProjectIds.Contains(t.ProjectId))
                .Count();
            var completedTasks = _context.Tasks
                .Where(t => managedProjectIds.Contains(t.ProjectId) && t.Status == "Done")
                .Count();
            var progress = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
            ProgressReportText.Text = $"{progress:F2}% completed ({completedTasks}/{totalTasks} tasks)";
        }





        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CurrentUser.Instance.Clear();
                Window mainWindow = Window.GetWindow(this);
                if (mainWindow is MainWindow mw)
                {
                    mw.ShowLoginForm();
                }
            }
        }

        private int? GetUserIdFromSelection()
        {
            // Placeholder: Thay bằng logic thực tế (ví dụ: ComboBox để chọn người dùng)
            // Ví dụ: Lấy từ danh sách người dùng trong cơ sở dữ liệu
            var users = _context.Users.ToList();
            // Cần thêm giao diện UI (ComboBox) để chọn người dùng
            return users.Any() ? users.First().UserId : (int?)null; // Trả về UserId đầu tiên nếu có
        }

        private string ShowStatusSelectionDialog()
        {
            // Placeholder: Hiển thị dialog để chọn trạng thái (ToDo, InProgress, Done)
            var statuses = new[] { "ToDo", "InProgress", "Done" };
            // Cần thêm giao diện UI (ComboBox hoặc dialog) để chọn trạng thái
            return statuses[0]; // Giả lập trả về "ToDo"
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            // Mở CreateProjectWindow
            var createProjectWindow = new CreateProjectWindow();
            createProjectWindow.Owner = Window.GetWindow(this);
            createProjectWindow.ProjectCreated += (name, description, startDate, endDate) =>
            {
                try
                {
                    // Bắt đầu transaction để đảm bảo toàn vẹn dữ liệu
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        // Tạo dự án mới
                        var newProject = new Project
                        {
                            Name = name,
                            Description = description,
                            StartDate = DateOnly.FromDateTime(startDate), // Convert DateTime to DateOnly
                            EndDate = endDate.HasValue ? DateOnly.FromDateTime(endDate.Value) : null, // Handle nullable DateTime
                            CreatedBy = GetCurrentUserId(),
                            CreatedAt = DateTime.Now
                        };

                        // Lưu dự án vào bảng Projects
                        _context.Projects.Add(newProject);
                        _context.SaveChanges(); // Lưu để lấy ProjectId

                        // Gán vai trò ProjectManager cho người tạo trong ProjectUserRoles
                        var projectUserRole = new ProjectUserRole
                        {
                            ProjectId = newProject.ProjectId,
                            UserId = GetCurrentUserId(),
                            Role = "ProjectManager",
                            AssignedAt = DateTime.Now
                        };

                        _context.ProjectUserRoles.Add(projectUserRole);
                        _context.SaveChanges();

                        // Commit transaction
                        transaction.Commit();

                        MessageBox.Show("Project created and role assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating project or assigning role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            createProjectWindow.ShowDialog();
        }

        int GetCurrentUserId()
        {
            var currentUser = CurrentUser.Instance.Current;

            return currentUser.UserId;


        }


    }
}
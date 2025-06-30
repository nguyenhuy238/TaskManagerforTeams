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
    /// Interaction logic for GuestView.xaml
    /// </summary>
    public partial class GuestView : UserControl
    {
        private readonly TaskManagerDb1Context _context;
        private int _currentGuestId; // Giả định UserId của Guest hiện tại

        public GuestView()
        {
            _context = new TaskManagerDb1Context();
            _currentGuestId = 3; // Giả lập UserId cho Guest, cần thay bằng logic thực tế
            InitializeComponent();
            LoadProjects();
            LoadTasks();
        }

        private void LoadProjects()
        {
            try
            {
                var accessibleProjects = _context.ProjectUserRoles
                    .Where(pur => pur.UserId == _currentGuestId && pur.Role == "Guest")
                    .Select(pur => pur.Project)
                    .ToList();

                ProjectsDataGrid.ItemsSource = accessibleProjects;
                if (accessibleProjects.Count == 0)
                {
                    MessageBox.Show("No projects accessible for the guest.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading projects: {ex.Message}");
            }
        }

        private void LoadTasks()
        {
            try
            {
                var accessibleProjectIds = _context.ProjectUserRoles
                    .Where(pur => pur.UserId == _currentGuestId && pur.Role == "Guest")
                    .Select(pur => pur.ProjectId)
                    .ToList();

                var tasks = _context.Tasks
                    .Where(t => accessibleProjectIds.Contains(t.ProjectId))
                    .Select(t => new
                    {
                        t.TaskId,
                        t.Title,
                        t.Status,
                        t.DueDate
                    }).ToList();

                TasksDataGrid.ItemsSource = tasks;
                if (tasks.Count == 0)
                {
                    MessageBox.Show("No tasks accessible for the guest.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}");
            }
        }
    }
}
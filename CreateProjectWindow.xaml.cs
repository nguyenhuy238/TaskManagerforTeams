using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CreateProjectWindow.xaml
    /// </summary>
    public partial class CreateProjectWindow : Window, INotifyPropertyChanged
    {
        private readonly TaskManagerDb1Context _context;
        private readonly int? _projectId;
        private string _windowTitle = "Create New Project";
        private string _saveButtonText = "Create";
        private string _projectName;
        private string _projectDescription;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public event Action<string, string, DateTime, DateTime?> ProjectCreated;

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        public string SaveButtonText
        {
            get => _saveButtonText;
            set
            {
                _saveButtonText = value;
                OnPropertyChanged(nameof(SaveButtonText));
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        public string ProjectDescription
        {
            get => _projectDescription;
            set
            {
                _projectDescription = value;
                OnPropertyChanged(nameof(ProjectDescription));
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public CreateProjectWindow(int? projectId = null)
        {
            InitializeComponent();
            _context = new TaskManagerDb1Context();
            _projectId = projectId;
            DataContext = this;

            if (_projectId.HasValue)
            {
                LoadProjectData();
            }
        }

        private void LoadProjectData()
        {
            var project = _context.Projects.Find(_projectId);
            if (project != null)
            {
                WindowTitle = "Edit Project";
                SaveButtonText = "Save";
                ProjectName = project.Name;
                ProjectDescription = project.Description;
                StartDate = project.StartDate.ToDateTime(TimeOnly.MinValue);
                EndDate = project.EndDate.HasValue ? project.EndDate.Value.ToDateTime(TimeOnly.MinValue) : null;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Project Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (dpStartDate.SelectedDate == null)
            {
                MessageBox.Show("Start Date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gọi sự kiện ProjectCreated với dữ liệu
            ProjectCreated?.Invoke(
                txtName.Text,
                txtDescription.Text,
                dpStartDate.SelectedDate.Value,
                dpEndDate.SelectedDate
            );

            // Đóng window
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

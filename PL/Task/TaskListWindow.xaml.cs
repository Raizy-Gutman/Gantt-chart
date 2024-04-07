using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
 
        public static readonly DependencyProperty IsForSelectionProperty = DependencyProperty.Register(
              "IsForSelection",
              typeof(bool),
              typeof(TaskListWindow ),
              new PropertyMetadata(null)
            );


        public bool IsForSelection
        {
            get { return (bool)GetValue(IsForSelectionProperty); }
            set { SetValue(IsForSelectionProperty, value); }
        }


        public TaskInList  SelectedTask { get; set; }

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Complexity { get; set; } = BO.EngineerExperience.None;

        public ObservableCollection<TaskInList> TaskList
        {
            get { return (ObservableCollection<TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = new(s_bl.Task.ReadAllTasks().Select(e => new TaskInList { Id = e.Id, Description = e.Description, Alias = e.Alias, Status = e.Status }));
        }

        private void ComplexitySelector_SelectionChanged(object sender, EventArgs e)
        {
            var taskInLists = (Complexity == BO.EngineerExperience.None) ?
                s_bl.Task.ReadAllTasks() :
                s_bl.Task.ReadAllTasks(e => (int)e.ComplexityLevel == (int)Complexity && !e.IsMilestone)!;

            ObservableCollection<TaskInList> newTaskList = new(
                    taskInLists.Select(e => new TaskInList { Id = e.Id, Description = e.Description, Alias = e.Alias, Status = e.Status }));
            TaskList = newTaskList;
        }

        private void ShowWindowAddTask_Click(object sender, RoutedEventArgs e)
        {
            var taskWindow = new Task.TaskWindow();
            taskWindow.Closed +=ComplexitySelector_SelectionChanged!;
            taskWindow.ShowDialog();
        }

     

        private void ToUpdateTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!IsForSelection)
            {
                TaskInList taskInList = ((sender as ListView)!.SelectedItem as TaskInList)!;
                var TaskWindow = new Task.TaskWindow(taskInList.Id);
                TaskWindow.Closed += ComplexitySelector_SelectionChanged!;
                TaskWindow.ShowDialog();
            }
            else
            {
                SelectedTask= ((sender as ListView)!.SelectedItem as TaskInList)!;
                this.Close();
            }
        }
    }
}

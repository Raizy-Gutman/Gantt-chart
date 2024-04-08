using BO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.Milestone
{
    /// <summary>
    /// Interaction logic for MilestoneList.xaml
    /// </summary>
    public partial class MilestoneList : Window
    {       
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MilestoneList()
        {
            InitializeComponent();
        }

        public MilestoneInList SelectedMilestone { get; set; }


        public BO.Status status { get; set; } = BO.Status.Unscheduled;

        public ObservableCollection<MilestoneInList> MilestonesList
        {
            get { return (ObservableCollection<MilestoneInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<MilestoneInList>), typeof(MilestoneList), new PropertyMetadata(null));


        //private void StatusSelector_SelectionChanged(object sender, EventArgs e)
        //{
        //    var taskInLists = (Complexity == BO.EngineerExperience.None) ?
        //        s_bl.Task.ReadAllTasks() :
        //        s_bl.Task.ReadAllTasks(e => (int)e.ComplexityLevel == (int)Complexity && !e.IsMilestone)!;

        //    ObservableCollection<TaskInList> newTaskList = new(
        //            taskInLists.Select(e => new TaskInList { Id = e.Id, Description = e.Description, Alias = e.Alias, Status = e.Status }));
        //    MilestonesList = newTaskList;
        //}

        //private void ShowWindowAddTask_Click(object sender, RoutedEventArgs e)
        //{
        //    var taskWindow = new Task.TaskWindow();
        //    taskWindow.Closed += StatusSelector_SelectionChanged!;
        //    taskWindow.ShowDialog();
        //}


        //private void ToUpdateTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (!IsForSelection)
        //    {
        //        TaskInList taskInList = ((sender as ListView)!.SelectedItem as TaskInList)!;
        //        var TaskWindow = new Task.TaskWindow(taskInList.Id);
        //        TaskWindow.Closed += StatusSelector_SelectionChanged!;
        //        TaskWindow.ShowDialog();
        //    }
        //    else
        //    {
        //        SelectedMilestone = ((sender as ListView)!.SelectedItem as TaskInList)!;
        //        this.Close();
        //    }
        //}
    }
}

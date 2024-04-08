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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerView.xaml
    /// </summary>
    public partial class EngineerView : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(EngineerView), new PropertyMetadata(null));
        public EngineerView(int Id)
        {
            InitializeComponent();
            if (s_bl.Engineer.GetEngineer(Id).Task is not null) 
            {
                try
                {
                    CurrentTask = s_bl.Task.GetTask(s_bl.Engineer.GetEngineer(Id).Task.Id);
                    if (CurrentTask.Milestone == null) CurrentTask.Milestone = new BO.MilestoneInTask() { Alias="milston alias is empty" };
                    if (CurrentTask.Engineer == null) CurrentTask.Engineer = new BO.EngineerInTask();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("There is no task assigned for you, please select a task", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}

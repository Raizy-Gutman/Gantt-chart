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

namespace PL
{

    /// <summary>
    /// Interaction logic for Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(Manager), new PropertyMetadata(null));

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime), typeof(Manager), new PropertyMetadata(null));

        public Visibility InputMode
        {
            get { return (Visibility)GetValue(InputModeProperty); }
            set { SetValue(InputModeProperty, value); }
        }

        public static readonly DependencyProperty InputModeProperty =
            DependencyProperty.Register("InputMode", typeof(Visibility), typeof(Manager), new PropertyMetadata(null));

        public Visibility InalizationMode
        {
            get { return (Visibility)GetValue(InalizationModeProperty); }
            set { SetValue(InalizationModeProperty, value); }
        }

        public static readonly DependencyProperty InalizationModeProperty =
            DependencyProperty.Register("InalizationMode", typeof(Visibility), typeof(Manager), new PropertyMetadata(null));

        public DateOnly CurrentDate = s_bl.CurrentDate;

        public void AddDay(object sender, RoutedEventArgs e) => s_bl.AddDay();

        public void ResetDate(object sender, RoutedEventArgs e) => s_bl.ResetDate();

        public Manager()
        {
            InitializeComponent();
            InalizationMode = s_bl.Engineer.ReadAllEngineers().ToArray().Length > 0 ? Visibility.Hidden : Visibility.Visible;
            InputMode = Visibility.Hidden;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(2);
        }

        private void Buttoninitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the database?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                s_bl.InitializeDB();
                InalizationMode = Visibility.Hidden;
                MessageBox.Show("Database initialization was successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("The initialization was cancelled.");
            }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the database? \n All data will be deleted!", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
                InalizationMode = Visibility.Visible;

                MessageBox.Show("Database successfully reset.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("The reset has been canceled.", "Information");

            }
        }

        private void EngineersListButton_Click(object sender, RoutedEventArgs e) => new Engineer.EngineerListWindow().Show();

        private void TasksListButton_Click(object sender, RoutedEventArgs e)
        {
            new Task.TaskListWindow().Show();
        }

        private void CreateSchedualButton_Click(object sender, RoutedEventArgs e)
        {
            InputMode = Visibility.Hidden;
            try
            {
                s_bl.Milestone.CreateSchedule(StartDate, EndDate);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonGetDates_Click(object sender, RoutedEventArgs e)
        {
            InputMode = Visibility.Visible;
        }

    }
}
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonEngineers_Click(object sender, RoutedEventArgs e)
        {
            new Engineer.EngineerListWindow().Show();
        }

        private void Buttoninitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult  result = MessageBox.Show("Are you sure you want to perform this action?", "Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes )
            {
                s_bl.InitializeDB();
                //(); // פעולה שנרצה להפעיל במידה והמשתמש אישר
            }
            else
            {
                MessageBox.Show("The operation was cancelled.");
            }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to Reset the database? \n All data will be deleted!", "Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
            else
            {
                MessageBox.Show("The operation was cancelled.");
            }
        }
    }
}

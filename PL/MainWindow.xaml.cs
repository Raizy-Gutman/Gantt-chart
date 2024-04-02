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
            MessageBoxResult  result = MessageBox.Show("Are you sure you want to initialize the database?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes )
            {
                s_bl.InitializeDB();
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
                MessageBox.Show("Database successfully reset.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("The reset has been canceled.", "Information");

            }
        }
    }
}

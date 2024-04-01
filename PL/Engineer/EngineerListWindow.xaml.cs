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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
        
        public ObservableCollection<BO.EngineerInList> EngineerList
        {
            get { return (ObservableCollection<BO.EngineerInList>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }
       
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.EngineerInList>), typeof(EngineerListWindow), new PropertyMetadata(null));       

        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = new ObservableCollection<BO.EngineerInList>(s_bl!.Engineer.ReadAllEngineers());
        }

        private void LevelSelector_SelectionChanged(object sender, EventArgs e)
        {
            var engineerInLists = (Level == BO.EngineerExperience.None) ? 
                s_bl.Engineer.ReadAllEngineers() :
                s_bl.Engineer.ReadAllEngineers(e => (int)e.Level == (int)Level)!;

            ObservableCollection<BO.EngineerInList> newEngineerList = new(engineerInLists);
            EngineerList = newEngineerList;
        }

        private void ShowWindowAddEngineer_Click(object sender, RoutedEventArgs e)
        {
            var singleEngineerWindow = new SingleEngineer.SingleEngineerWindow();
            singleEngineerWindow.Closed += LevelSelector_SelectionChanged!;
            singleEngineerWindow.ShowDialog();
        }

        private void ToUpdateEngineer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.EngineerInList? engineerInList = (sender as ListView)!.SelectedItem as BO.EngineerInList;
            var singleEngineerWindow = new SingleEngineer.SingleEngineerWindow(engineerInList!.Id);
            singleEngineerWindow.Closed += LevelSelector_SelectionChanged!;
            singleEngineerWindow.ShowDialog();
        }
    }
}
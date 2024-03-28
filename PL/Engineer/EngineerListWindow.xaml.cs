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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAllEngineers()!;
        }

        public IEnumerable<BO.EngineerInList> EngineerList
        {
            get { return (IEnumerable<BO.EngineerInList>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.EngineerInList>), typeof(EngineerListWindow), new PropertyMetadata(null));

        private void LevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var engineerInLists = (Level == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAllEngineers()! : s_bl?.Engineer.ReadAllEngineers()!;

            var v =  engineerInLists.Select(a => s_bl?.Engineer.GetEngineer(a.Id)).Where(e => e?.Level==Level).Select(a=>a?.Id).ToList();
            EngineerList=engineerInLists.Where(a => v.Contains ( a.Id));
        }

        private void ShowWindowAddEngineer_Click(object sender, RoutedEventArgs e)
        {
            new SingleEngineer.SingleEngineerWindow().ShowDialog();
        }

        private void ToUpdateEngineer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO. EngineerInList? engineerInList = (sender as ListView)?.SelectedItem as BO.EngineerInList;
            new SingleEngineer.SingleEngineerWindow(engineerInList!.Id).ShowDialog();
        }
    }
}
using PL.Engineer;
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

namespace PL.SingleEngineer
{
    /// <summary>
    /// Interaction logic for SingleEngineerWindow.xaml
    /// </summary>
    public partial class SingleEngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public SingleEngineerWindow(int Id = 0)
        {
            InitializeComponent();
            if (Id == 0)
            {
                CurrentEngineer = new BO.Engineer();
            }
            else
            {
                try
                {
                    CurrentEngineer = s_bl.Engineer.GetEngineer(Id);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(SingleEngineerWindow), new PropertyMetadata(null));

        private void BtnAddOrUpdateEngineer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try 
            {
                if ((string)button!.Content == "Add")
                {
                    // Creating the new engineer in the BL
                    s_bl.Engineer.CreateEngineer(CurrentEngineer);
                    MessageBox.Show("The engineer has been added successfully!");
                }
                else
                {
                    // Updating the existing engineer
                    s_bl.Engineer.UpdateEngineer(CurrentEngineer);
                    MessageBox.Show("The engineer has been updated successfully!");
                }
                // Closing the window
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("There is a problem adding/updating the engineer" + ex.Message);
            }
        }
    }
}

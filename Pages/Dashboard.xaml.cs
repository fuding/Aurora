using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;

namespace Aurora.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        MainWindow ParentWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);
        public Dashboard()
        {
            InitializeComponent();
        }

        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonCalibration_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Panel.Clear();
            ParentWindow.Panel.Add(new Calibration());
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Panel.Clear();
            ParentWindow.Panel.Add(new Settings());
        }
    }
}

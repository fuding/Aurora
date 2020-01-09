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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aurora.Pages
{
    /// <summary>
    /// Interaction logic for Calibration.xaml
    /// </summary>
    public partial class Calibration : UserControl
    {
        MainWindow ParentWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);
        CalibrationWindow calibrationWindow = new CalibrationWindow();

        bool calibration_running = false;
        public Calibration()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            if(!calibration_running)
            {
                startButtonText.Text = "Stop calibration";
                int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;



                calibrationWindow.Width = screenWidth;
                calibrationWindow.Height = screenHeight;

                calibrationWindow.WindowStartupLocation = WindowStartupLocation.Manual;

                calibrationWindow.Left = 0;
                calibrationWindow.Top = 0;

                calibrationWindow.Show();
                Application.Current.MainWindow.Owner = calibrationWindow;


                calibration_running = true;
            }
            else
            {
                startButtonText.Text = "Run calibration";
                calibrationWindow.Hide();
                calibration_running = false;
            }
            
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.showMenu();
            ParentWindow.Panel.Add(new Dashboard());
        }
    }
}

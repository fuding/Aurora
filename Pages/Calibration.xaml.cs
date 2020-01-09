using System.Windows;
using System.Windows.Controls;

namespace Aurora.Pages
{
    /// <summary>
    /// Interaction logic for Calibration.xaml
    /// </summary>
    public partial class Calibration : UserControl
    {
        MainWindow ParentWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);

        bool calibration_running = false;
        public Calibration()
        {
            InitializeComponent();
        }

        private void stopCalibration()
        {
            if (calibration_running)
            {
                startButtonText.Text = "Run calibration";
                ParentWindow.hideCalibration();
                calibration_running = false;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            stopCalibration();
        }

        private void ButtonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            if(!calibration_running)
            {
                startButtonText.Text = "Save calibration";
                ParentWindow.showCalibration();
                calibration_running = true;
            }
            else
            {
                stopCalibration();
            }
            
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            stopCalibration();

            ParentWindow.showMenu();
            ParentWindow.Panel.Add(new Dashboard());
        }
    }
}

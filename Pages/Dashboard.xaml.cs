using System.Collections.Generic;
using System.Windows.Controls;

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
            Preview();
            DisplayStatus();
        }

        private void DisplayStatus()
        {
            if (ParentWindow.status)
                currentStatus.Text = "Enabled";
            else
                currentStatus.Text = "Disabled";
        }

        private void Preview()
        {
            /*
            List<int> horizontalPixelArray = new List<int> { };
            List<int> verticalPixelArray = new List<int> { };

            int screenW = ParentWindow.screenSource.getWidth();
            int screenH = ParentWindow.screenSource.getHeight();

            int pixelDistanceW = screenW / Properties.Settings.Default.top_led;
            int pixelDistanceH = screenH / Properties.Settings.Default.side_led;

            int currentPixel = 0;
            for (int i = 0; i < Properties.Settings.Default.top_led; i++)
            {
                horizontalPixelArray.Add(currentPixel += pixelDistanceW);
            }

            currentPixel = 0;
            for (int i = 0; i < Properties.Settings.Default.top_led; i++)
            {
                if(currentPixel + pixelDistanceW < screenW)
                    verticalPixelArray.Add(currentPixel += pixelDistanceW);
            }
            */
        }
    }
}

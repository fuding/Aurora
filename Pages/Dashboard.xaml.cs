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

            fillLeds();
        }

        public void fillLeds()
        {
            for (int i = 0; i < 20; i++)
            {


                TopLedsList.Children.Add(new ContentControl() { ContentTemplate = (System.Windows.DataTemplate)this.Resources["LED_BOX"] });
            }
        }

        private void DisplayStatus()
        {
            if (ParentWindow.status)
                currentStatus.Text = "Enabled";
            else
                currentStatus.Text = "Disabled";

            if(Properties.Settings.Default.serial_port != "")
                serialPort.Text = Properties.Settings.Default.serial_port;
            else
                serialPort.Text = "Unknown";
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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;

struct LedColor
{
    public string raw_color { get; set; }
}

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
            fillLeds();
        }

        public void fillLeds()
        {
            List<LedColor> ledlist = new List<LedColor> { };

            System.Drawing.Color[] pixels = ParentWindow.screenSource.getTopColors();

            foreach (System.Drawing.Color pixel in pixels)
                ledlist.Add(new LedColor() { raw_color = ColorTranslator.ToHtml(pixel) });

            topledlist.ItemsSource = ledlist;
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

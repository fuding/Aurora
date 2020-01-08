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

            if (ParentWindow.status == true)
                statusText.Text = "Stop service";
            else
                statusText.Text = "Start service";

            ticksCount.Text = "Ticks: " + ParentWindow.ticks.ToString();

            Preview();

            ParentWindow.screenSource.Refresh();
            DEBUGIMAGE.Source = ParentWindow.screenSource.debugPreview();
        }

        private void Preview()
        {
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

            string debText = "Resolution: " + screenW + "/" + screenH + "\nDystans pixeli w poziomie: " + pixelDistanceW.ToString() + "\nDystans pixeli w pionie: " + pixelDistanceH.ToString();

            debText += "\n\nLista pixeli\n";
            foreach (int pixel in horizontalPixelArray)
            {
                System.Drawing.Color pixelColor = ParentWindow.screenSource.getTop(pixel);
                debText += "\nHorP - " + pixel + ":" + pixelDistanceH + " = R: " + pixelColor.R + ", G: " + pixelColor.G + ", B: " + pixelColor.B;
            }

            foreach (int pixel in verticalPixelArray)
            {
                //System.Drawing.Color pixelColor = ParentWindow.screenSource.getPixel(15, pixel);
                //debText += "\nHorP - " + pixel + ":15 = R: " + pixelColor.R + ", G: " + pixelColor.G + ", B: " + pixelColor.B;
            }

            debugText.Text = debText;

        }

        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            if (ParentWindow.status == false)
            {
                ParentWindow.status = true;
                //statusText.Text = "Stop service";
            }
            else
            {
                ParentWindow.status = false;
                //statusText.Text = "Start service";
            }
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

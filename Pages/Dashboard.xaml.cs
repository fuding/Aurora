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
        }

        private void Preview()
        {
            int[,] pixelArray;

            int screenW = ParentWindow.screenSource.getWidth();
            int screenH = ParentWindow.screenSource.getHeight();

            int pixelDistanceW = screenW / Properties.Settings.Default.top_led;
            int pixelDistanceH = screenH / Properties.Settings.Default.side_led;

            portsText.Text = "Dystans pixeli w poziomie: " + pixelDistanceW.ToString() + "\nDystans pixeli w pionie: " + pixelDistanceH.ToString();


            System.Drawing.Color pixel0 = ParentWindow.screenSource.getPixel(1, 1);
            System.Drawing.Color pixel1 = ParentWindow.screenSource.getPixel(100, 100);
            System.Drawing.Color pixel2 = ParentWindow.screenSource.getPixel(500, 500);
            System.Drawing.Color pixel3 = ParentWindow.screenSource.getPixel(1000, 1000);
            System.Drawing.Color pixel4 = ParentWindow.screenSource.getPixel(ParentWindow.screenSource.getWidth() - 1, ParentWindow.screenSource.getHeight() - 1);

            string pixels = "\n\nSample Pixels info\n";
            pixels += "\nPixel 1x1: R: " + pixel0.R + ", G: " + pixel0.G + ", B: " + pixel0.B;
            pixels += "\nPixel 100x100: R: " + pixel1.R + ", G: " + pixel1.G + ", B: " + pixel1.B;
            pixels += "\nPixel 500x500: R: " + pixel2.R + ", G: " + pixel2.G + ", B: " + pixel2.B;
            pixels += "\nPixel 1000x1000: R: " + pixel3.R + ", G: " + pixel3.G + ", B: " + pixel3.B;
            pixels += "\nPixel " + ParentWindow.screenSource.getWidth() + ":" + ParentWindow.screenSource.getHeight() + ": R: " + pixel4.R + ", G: " + pixel4.G + ", B: " + pixel4.B;

            pixelInfo.Text = pixels;

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

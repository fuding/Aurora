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
            Preview();
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

            debugText1.Text = "Resolution: " + screenW + "/" + screenH + "\nDystans pixeli w poziomie: " + pixelDistanceW.ToString() + "\nDystans pixeli w pionie: " + pixelDistanceH.ToString();

            string debText2 = "\n\nHorizontal top pixels\n";
            foreach (int pixel in horizontalPixelArray)
            {
                System.Drawing.Color pixelColor = ParentWindow.screenSource.getTop(pixel);
                debText2 += "\nHorP - " + pixel + ":" + pixelDistanceH + " = R: " + pixelColor.R + ", G: " + pixelColor.G + ", B: " + pixelColor.B;
            }
            debugText2.Text = debText2;

            string debText3 = "\n\nHorizontal bottom pixels\n";
            foreach (int pixel in verticalPixelArray)
            {
                System.Drawing.Color pixelColor = ParentWindow.screenSource.getBottom(pixel);
                debText3 += "\nHorP - " + pixel + ":15 = R: " + pixelColor.R + ", G: " + pixelColor.G + ", B: " + pixelColor.B;
            }
            debugText3.Text = debText3;

        }
    }
}

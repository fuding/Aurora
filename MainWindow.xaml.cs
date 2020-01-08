using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Aurora.Assets;

namespace Aurora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int tickRate = 200;
        public MainWindow()
        {
            InitializeComponent();

            AsyncScreenshot();

            scWidth.Text = Screen.PrimaryScreen.Bounds.Width.ToString();
            scHeight.Text = Screen.PrimaryScreen.Bounds.Height.ToString();
        }

        private void AsyncScreenshot()
        {
            int counter = 0;
            ScreenColor screenSource = new ScreenColor();
            _ = Task.Factory.StartNew(() =>
              {
                  Action refreshAction = delegate
                  {
                      screenSource.Refresh();
                      counter++;
                  };
                  while (true)
                  {
                      System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, refreshAction);
                      this.Dispatcher.Invoke(() =>
                      {
                          scTest.Text = "Thread started, counter = " + counter;

                          System.Drawing.Color pixel = screenSource.getPixel(100, 100);
                          System.Drawing.Color pixel2 = screenSource.getPixel(500, 500);

                          scWidth.Text = "Pixel from 100 x 100 have color: R:" + pixel.R + ", G: " + pixel.G + ", B: " + pixel.B;
                          scHeight.Text = "Pixel from 500 x 500 have color: R:" + pixel2.R + ", G: " + pixel2.G + ", B: " + pixel2.B;
                      });
                      System.Threading.Thread.Sleep(tickRate);
                  }
              }
            );
        }
    }
}

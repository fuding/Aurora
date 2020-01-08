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
        ScreenColor screenSource;

        int tickRate = 200;
        public MainWindow()
        {
            InitializeComponent();
            Tray();

            screenSource = new ScreenColor();

            scInfo.Text = "Screen width: " + screenSource.getWidth() + "\nScreen Height: " + screenSource.getHeight();
            AsyncScreenshot();
        }
        private void Tray()
        {
            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
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
                          tickRateText.Text = "Ticks: " + counter;

                          System.Drawing.Color pixel = screenSource.getPixel(100, 100);
                          System.Drawing.Color pixel2 = screenSource.getPixel(500, 500);

                          //scWidth.Text = "Pixel from 100 x 100 have color: R:" + pixel.R + ", G: " + pixel.G + ", B: " + pixel.B;
                          //scHeight.Text = "Pixel from 500 x 500 have color: R:" + pixel2.R + ", G: " + pixel2.G + ", B: " + pixel2.B;
                      });
                      System.Threading.Thread.Sleep(tickRate);
                  }
              }
            );
        }
    }
}

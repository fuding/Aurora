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
using Aurora.Pages;

namespace Aurora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ScreenColor screenSource;
        UIElementCollection Panel;
        Dashboard Dashboard = new Dashboard();
        Calibration Calibration = new Calibration();
        Settings Settings = new Settings();

        int view = 0;
        int tickRate = 200;
        int status = 0;
        public MainWindow()
        {
            screenSource = new ScreenColor();

            Dashboard.changeInfo("Screen width: " + screenSource.getWidth() + "\nScreen Height: " + screenSource.getHeight());
            
            InitializeComponent();
            Panel = PagePanel.Children;

            Panel.Add(Dashboard);

            Tray();
            AsyncScreenshot();
        }
        private void Tray()
        {
            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
            ni.Visible = true;
            ni.DoubleClick += delegate (object sender, EventArgs args)
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

                          System.Drawing.Color pixel0 = screenSource.getPixel(1, 1); 
                          System.Drawing.Color pixel1 = screenSource.getPixel(100, 100);
                          System.Drawing.Color pixel2 = screenSource.getPixel(500, 500);
                          System.Drawing.Color pixel3 = screenSource.getPixel(1000, 1000);
                          System.Drawing.Color pixel4 = screenSource.getPixel(screenSource.getWidth() - 1, screenSource.getHeight() - 1);

                          string pixels = "\n\nSample Pixels info\n";
                          pixels += "\nPixel 1x1: R: " + pixel0.R + ", G: " + pixel0.G + ", B: " + pixel0.B;
                          pixels += "\nPixel 100x100: R: " + pixel1.R + ", G: " + pixel1.G + ", B: " + pixel1.B;
                          pixels += "\nPixel 500x500: R: " + pixel2.R + ", G: " + pixel2.G + ", B: " + pixel2.B;
                          pixels += "\nPixel 1000x1000: R: " + pixel3.R + ", G: " + pixel3.G + ", B: " + pixel3.B;
                          pixels += "\nPixel " + screenSource.getWidth() + ":" + screenSource.getHeight() + ": R: " + pixel4.R + ", G: " + pixel4.G + ", B: " + pixel4.B;

                          if(view == 0)
                          {
                              Dashboard.changePixel(pixels);
                              Panel.Clear();
                              Panel.Add(Dashboard);
                          }

                          //scWidth.Text = "Pixel from 100 x 100 have color: R:" + pixel.R + ", G: " + pixel.G + ", B: " + pixel.B;
                          //scHeight.Text = "Pixel from 500 x 500 have color: R:" + pixel2.R + ", G: " + pixel2.G + ", B: " + pixel2.B;
                      });
                      System.Threading.Thread.Sleep(tickRate);
                  }
              }
            );
        }
        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            if (view == 0)
            {
                if (status == 0)
                {
                    status = 1;
                    Button1Title.Text = "Stop";
                }
                else
                {
                    status = 0;
                    Button1Title.Text = "Start";
                }
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            if(view == 1)
            {
                view = 0;
                ButtonSettings.Visibility = Visibility.Visible;
                Button1Title.Text = (status == 1 ? "Stop" : "Start");
                Button2Title.Text = "Calibration";
                Button3Title.Text = "Settings";
                Panel.Clear();
                Panel.Add(Dashboard);
            }
            else
            {
                view = 2;
                ButtonSettings.Visibility = Visibility.Hidden;
                Button1Title.Text = "Save";
                Button2Title.Text = "Cancel";
                Panel.Clear();
                Panel.Add(Settings);
            }

        }

        private void ButtonCalibration_Click(object sender, RoutedEventArgs e)
        {
            
            if (view == 2)
            {
                view = 0;
                ButtonSettings.Visibility = Visibility.Visible;
                Button1Title.Text = (status == 1 ? "Stop" : "Start");
                Button2Title.Text = "Calibration";
                Button3Title.Text = "Settings";
                Panel.Clear();
                Panel.Add(Dashboard);
            }
            else
            {
                view = 1;
                Panel.Clear();
                Button1Title.Text = "Save";
                Button2Title.Text = "Run calibration";
                Button3Title.Text = "Cancel";
                Panel.Add(Calibration);
            }
        }
    }
}

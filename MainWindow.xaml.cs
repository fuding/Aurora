using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Aurora.Assets;
using Aurora.Pages;
using System.ComponentModel;
using System.Diagnostics;

namespace Aurora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon ni = new NotifyIcon();

        public UIElementCollection Panel;
        public SerialOutput serial = new SerialOutput();
        public ScreenColor screenSource = new ScreenColor();
        public CalibrationWindow calibrationWindow;

        Process currentProc = Process.GetCurrentProcess();
        public int tickRate = 100;
        public int ticks = 0;
        public bool status = false;
        public bool lockview = false;

        public MainWindow()
        {
            InitializeComponent();
            Tray();

            Panel = PagePanel.Children;
            Panel.Add(new Dashboard());

            AsyncScreenshot();

            //Ram usage
            ramUsage.Text = "RAM Usage: " + (currentProc.PrivateMemorySize64 / 1024 / 1024).ToString() + "MB";
        }
        private void Tray()
        {
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetEntryAssembly().ManifestModule.Name);
            ni.Visible = true;
            ni.DoubleClick += delegate (object sender, EventArgs args)
            {
                ni.Visible = false;

                this.Show();
                this.WindowState = WindowState.Normal;
            };
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                ni.Visible = true;
                this.Hide();
                if (calibrationWindow != null)
                    if (calibrationWindow.ShowActivated)
                        calibrationWindow.Hide();
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            status = false;
            Environment.Exit(0);
            base.OnClosing(e);
        }

        public void showCalibration()
        {
            if (calibrationWindow == null)
                calibrationWindow = new CalibrationWindow() { Top = 0, Left = 0, WindowStartupLocation = WindowStartupLocation.Manual };

            calibrationWindow.Width = screenSource.getWidth();
            calibrationWindow.Height = screenSource.getHeight();

            calibrationWindow.Show();
            this.Owner = calibrationWindow;
            this.Focus();
        }

        public void hideCalibration()
        {
            calibrationWindow.Hide();
            this.Focus();
        }

        private void AsyncScreenshot()
        {
            _ = Task.Factory.StartNew(() =>
              {
                  Action refreshAction = delegate
                  {
                      //screenSource.Refresh();
                      screenSource.Refresh(90);
                      
                      ticks++;
                  };
                  while (true)
                  {
                      if (status)
                      {
                          System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, refreshAction);
                          this.Dispatcher.Invoke(() =>
                          {
                              serial = new SerialOutput();

                              System.Drawing.Color[] pixels = screenSource.getTopColors();

                              byte[] fillled = new byte[(Properties.Settings.Default.top_led * 3)];
                              int cnt = 0;
                              foreach (System.Drawing.Color pixel in pixels)
                              {
                                  fillled[cnt++] = pixel.R;
                                  fillled[cnt++] = pixel.G;
                                  fillled[cnt++] = pixel.B;
                              }

                              serial.FillLEDs(fillled);
                              serial.Send();

                              if (!lockview)
                              {
                                  //Refresh ticks
                                  ticksCount.Text = "Ticks: " + ticks.ToString();

                                  //Ram usage
                                  currentProc = Process.GetCurrentProcess();
                                  ramUsage.Text = "RAM Usage: " + (currentProc.PrivateMemorySize64 / 1024 / 1024).ToString() + "MB";

                                  //Refresh preview on dashboard
                                  Panel.Clear();
                                  Panel.Add(new Dashboard());
                              }
                          });
                      }
                      System.Threading.Thread.Sleep(tickRate);
                  }
              }
            );
        }

        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            if (status == false)
            {
                status = true;
                statusText.Text = "Stop service";
            }
            else
            {
                status = false;
                statusText.Text = "Start service";
            }
        }

        private void clearPanel()
        {
            lockview = true;
            Panel.Clear();
            HomeNavigation.Visibility = Visibility.Hidden;
            HomeNavigation.Height = 0;
        }

        public void showMenu()
        {
            lockview = false;
            Panel.Clear();
            HomeNavigation.Visibility = Visibility.Visible;
            HomeNavigation.Height = Double.NaN;
        }

        private void ButtonCalibration_Click(object sender, RoutedEventArgs e)
        {
            clearPanel();
            Panel.Add(new Calibration());
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            clearPanel();
            Panel.Add(new Settings());
        }
    }
}

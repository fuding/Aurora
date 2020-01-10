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

        public Dashboard dash;

        public MainWindow()
        {
            InitializeComponent();
            Tray();
            AsyncScreenshot();

            FillText();

            dash = new Dashboard();
            Panel = PagePanel.Children;
            Panel.Add(dash);
        }

        private void FillText()
        {
            //RAM usage
            ramUsage.Text = "RAM Usage: " + (currentProc.PrivateMemorySize64 / 1024 / 1024).ToString() + "MB";

            //Selected serial port
            if (Properties.Settings.Default.serial_port != "")
                serialPort.Text = "Serial port: " + Properties.Settings.Default.serial_port;
            else
                serialPort.Text = "Serial port: " + "Unknown";
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
                  Action updateApplication = delegate
                  {
                      //Refresh ticks
                      ticksCount.Text = "Ticks: " + ticks.ToString();

                      if (!lockview)
                      {
                          //Refresh ticks
                          ticksCount.Text = "Ticks: " + ticks.ToString();

                          //Ram usage
                          currentProc = Process.GetCurrentProcess();
                          ramUsage.Text = "RAM Usage: " + (currentProc.PrivateMemorySize64 / 1024 / 1024).ToString() + "MB";

                          //Refresh preview on dashboard
                          dash.Update();
                      }
                  };

                  while (true)
                  {
                      if (status)
                      {
                          screenSource.Refresh(90);
                          System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, updateApplication);
                          ticks++;
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

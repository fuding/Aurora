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
        NotifyIcon ni = new NotifyIcon();

        public UIElementCollection Panel;
        public ScreenColor screenSource = new ScreenColor();
        public int tickRate = 150;
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
            }
            base.OnStateChanged(e);
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
                              //SerialOutput serial = new SerialOutput();
                              //serial.Send();

                              if (!lockview)
                              {
                                  //Refresh ticks
                                  ticksCount.Text = "Ticks: " + ticks.ToString();

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

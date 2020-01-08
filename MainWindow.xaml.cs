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
        public int tickRate = 250;
        public int ticks = 0;
        public bool status = false;

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
                      ticks++;
                  };
                  while (true)
                  {
                      if (status)
                      {
                          System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, refreshAction);
                          this.Dispatcher.Invoke(() =>
                          {
                              SerialOutput serial = new SerialOutput();
                              serial.Send();

                              //Refresh preview on dashboard
                              Panel.Clear();
                              Panel.Add(new Dashboard());
                          });
                      }
                      System.Threading.Thread.Sleep(tickRate);
                  }
              }
            );
        }
    }
}

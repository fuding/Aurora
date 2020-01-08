using System;
using System.Collections.Generic;
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
using System.IO.Ports;
using System.Windows.Shapes;

namespace Aurora.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        MainWindow ParentWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);
        private string selectedPort;

        public Settings()
        {
            InitializeComponent();

            updatePorts();

            top_led.Text = Properties.Settings.Default.top_led.ToString();
            side_led.Text = Properties.Settings.Default.side_led.ToString();

            led_dir.SelectedIndex = Properties.Settings.Default.led_dir;
            led_input.SelectedIndex = Properties.Settings.Default.led_input;

            if (Properties.Settings.Default.active_left)
                cb1.IsChecked = true;

            if (Properties.Settings.Default.active_right)
                cb2.IsChecked = true;

            if (Properties.Settings.Default.active_top)
                cb3.IsChecked = true;

            if (Properties.Settings.Default.active_bottom)
                cb4.IsChecked = true;
        }

        public void updatePorts()
        {
            serial_port.ItemsSource = null;
            serial_port.Items.Clear();

            string[] portsList = SerialPort.GetPortNames();

            foreach(string port in portsList)
            {
                serial_port.Items.Add(port);
            }

            serial_port.Items.Refresh();
        }

        private bool handle = true;
        private void portsClosed(object sender, EventArgs e)
        {
            if (handle) handleComboChange();
            handle = true;
        }

        private void portsChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            handleComboChange();
        }
        private void handleComboChange()
        {
            //Shit dunno work
            selectedPort = serial_port.SelectedItem.ToString();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            if (!Int32.TryParse(top_led.Text, out i))
                i = -1;
            Properties.Settings.Default.top_led = i;

            if (!Int32.TryParse(side_led.Text, out i))
                i = -1;
            Properties.Settings.Default.side_led = i;

            Properties.Settings.Default.led_dir = led_dir.SelectedIndex;
            Properties.Settings.Default.led_input = led_input.SelectedIndex;

            Properties.Settings.Default.active_left = cb1.IsChecked.Value;
            Properties.Settings.Default.active_right = cb2.IsChecked.Value;
            Properties.Settings.Default.active_top = cb3.IsChecked.Value;
            Properties.Settings.Default.active_bottom = cb4.IsChecked.Value;

            Properties.Settings.Default.serial_port = selectedPort;

            Properties.Settings.Default.Save();

            ParentWindow.showMenu();
            ParentWindow.Panel.Add(new Dashboard());
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.showMenu();
            ParentWindow.Panel.Add(new Dashboard());
        }
    }
}

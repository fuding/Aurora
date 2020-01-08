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
using System.Windows.Shapes;

namespace Aurora.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();

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

        public int getTopLedCount()
        {
            int i = 0;
            if (!Int32.TryParse(top_led.Text, out i))
                i = -1;
            return i;
        }

        public int getSideLedCount()
        {
            int i = 0;
            if (!Int32.TryParse(side_led.Text, out i))
                i = -1;
            return i;
        }

        public int getLedDir()
        {
            return led_dir.SelectedIndex;
        }
        
        public int getLedInput()
        {
            return led_input.SelectedIndex;
        }

        public bool[] getActiveBars()
        {
            return new bool[] { cb1.IsChecked.Value, cb2.IsChecked.Value, cb3.IsChecked.Value, cb4.IsChecked.Value };
        }
    }
}

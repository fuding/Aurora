using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;

struct LedColor
{
    public string raw_color { get; set; }
}

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

        public void Update()
        {
            Preview();
        }

        private void Preview()
        {
            //Top led list
            List<LedColor> ledlist = new List<LedColor> { };
            System.Drawing.Color[] pixels = ParentWindow.screenSource.getTopColors();
            foreach (System.Drawing.Color pixel in pixels)
                ledlist.Add(new LedColor() { raw_color = ColorTranslator.ToHtml(pixel) });
            topledlist.ItemsSource = ledlist;

            //Bottom led list
            ledlist = new List<LedColor> { };
            pixels = ParentWindow.screenSource.getBottomColors();
            foreach (System.Drawing.Color pixel in pixels)
                ledlist.Add(new LedColor() { raw_color = ColorTranslator.ToHtml(pixel) });
            bottomledlist.ItemsSource = ledlist;
        }
    }
}

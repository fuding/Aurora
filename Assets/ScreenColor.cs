using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Aurora.Assets
{
    class ScreenColor
    {
        Bitmap bitmap_screen;
        Graphics graphic_screen;

        int screenWidth = 0;
        int screenHeight = 0;

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ScreenColor()
        {
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;

            this.takeScreenshot();
        }

        public void Refresh()
        {
            this.takeScreenshot();
        }

        public System.Drawing.Color getPixel(int x = 1, int y = 1)
        {
            return bitmap_screen.GetPixel(x, y);
        }

        public ImageSource getPreview()
        {
            IntPtr handle = bitmap_screen.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void takeScreenshot()
        {
            bitmap_screen = new Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            graphic_screen = Graphics.FromImage(bitmap_screen);
            graphic_screen.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            
            MemoryStream msIn = new MemoryStream();
            bitmap_screen.Save(msIn, System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()[0], null);
        }

    }
}

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
    public class ScreenColor
    {
        int screenWidth = 0;
        int screenHeight = 0;

        Bitmap[] bmp_map = new Bitmap[4];

        public ScreenColor()
        {
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;

            bmp_map[0] = new Bitmap(screenWidth, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmp_map[1] = new Bitmap(screenWidth, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            bmp_map[2] = new Bitmap(1, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmp_map[3] = new Bitmap(1, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }


        public System.Drawing.Color[] getTopColors()
        {
            //Led count
            System.Drawing.Color[] colors = new System.Drawing.Color[Properties.Settings.Default.top_led];

            //Screen size without padding
            int workWidth = screenWidth - (Properties.Settings.Default.top_padding * 2 );

            //Current pixel position
            int currentPosition = Properties.Settings.Default.top_padding;

            //Pixel multiplier
            int pixelDistance = workWidth / Properties.Settings.Default.top_led;

            for (int i = 0; i < Properties.Settings.Default.top_led; i++)
            {
                
                if(currentPosition >= screenWidth)
                {
                    colors[i] = getTop(screenWidth - 1);
                }
                else
                {
                    colors[i] = getTop(currentPosition);
                    currentPosition += pixelDistance;
                }
            }

            /* Debug
            int cnt = 0;
            foreach (System.Drawing.Color item in colors)
            {
                cnt++;
                Console.WriteLine("=======================\nPOSITION\nCurrent position #" + cnt + ", R:" + item.R + ", G:" + item.G + ", B:" + item.B);
            }
            */

            return colors;
        }

        public void Refresh(int horizontalOffset = 20, int verticalOffset = 20)
        {
            refreshTop(horizontalOffset);
            refreshBottom(horizontalOffset);
        }

        public void refreshTop(int offset = 20)
        {
            Graphics gfx = Graphics.FromImage(bmp_map[0]);
            gfx.CopyFromScreen(0, offset, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            MemoryStream msIn = new MemoryStream();
            bmp_map[0].Save(msIn, System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()[0], null);
        }

        public void refreshBottom(int offset = 20)
        {
            Graphics gfx = Graphics.FromImage(bmp_map[1]);
            gfx.CopyFromScreen(0, screenHeight - offset, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            MemoryStream msIn = new MemoryStream();
            bmp_map[1].Save(msIn, System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()[0], null);
        }

        public System.Drawing.Color getTop(int position = 1)
        {
            return bmp_map[0].GetPixel(position, 0);
        }

        public System.Drawing.Color getBottom(int position = 1)
        {
            return bmp_map[1].GetPixel(position, 0);
        }

        public System.Drawing.Color getLeft(int position = 1)
        {
            return bmp_map[1].GetPixel(0, position);
        }

        public System.Drawing.Color getRight(int position = 1)
        {
            return bmp_map[1].GetPixel(0, position);
        }

        public int getHeight()
        {
            return screenHeight;
        }

        public int getWidth()
        {
            return screenWidth;
        }
    }
}

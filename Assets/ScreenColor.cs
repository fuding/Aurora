using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Aurora.Assets
{
    public class ScreenColor
    {
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        Bitmap[] bmp_map = new Bitmap[4];

        public ScreenColor()
        {
            BMPInit();
        }

        public int getHeight()
        {
            return screenHeight;
        }

        public int getWidth()
        {
            return screenWidth;
        }

        private void BMPInit()
        {
            bmp_map[0] = new Bitmap(screenWidth, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmp_map[1] = new Bitmap(screenWidth, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            bmp_map[2] = new Bitmap(1, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            bmp_map[3] = new Bitmap(1, screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public void Refresh(int horizontalOffset = 20, int verticalOffset = 20)
        {
            refreshTop(horizontalOffset);
            refreshBottom(horizontalOffset);
        }

        public System.Drawing.Color[] getTopColors()
        {
            System.Drawing.Color[] colors = new System.Drawing.Color[Properties.Settings.Default.top_led];
            int currentPosition = Properties.Settings.Default.top_padding;
            int pixelDistance = (screenWidth - (Properties.Settings.Default.top_padding * 2)) / Properties.Settings.Default.top_led;

            for (int i = 0; i < Properties.Settings.Default.top_led; i++)
            {
                if(currentPosition >= screenWidth)
                    colors[i] = bmp_map[0].GetPixel(screenWidth - 1, 0);
                else
                    colors[i] = bmp_map[0].GetPixel(currentPosition, 0);
                    currentPosition += pixelDistance;
            }

            return colors;
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
    }
}

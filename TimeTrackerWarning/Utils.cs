using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTrackerWarning
{
    static class Utils
    {
        public static Bitmap GetNotificationIconsImage()
        {
            const int WIDTH = 400;
            const int HEIGHT = 130; // vertical notification area is smaller, two rows

            var taskBarLocation = GetTaskBarLocation();
            var ps = Screen.PrimaryScreen;
            switch (taskBarLocation)
            {
                // Horizontal taskbar
                case TaskBarLocation.BOTTOM:
                    return Capture(
                        ps.WorkingArea.Right - WIDTH,
                        ps.WorkingArea.Bottom,
                        WIDTH,
                        ps.Bounds.Height - ps.WorkingArea.Height);
                case TaskBarLocation.TOP:
                    return Capture(
                        ps.WorkingArea.Right - WIDTH,
                        0,
                        WIDTH,
                        ps.Bounds.Height - ps.WorkingArea.Height);
                
                // Vertical taskbar
                case TaskBarLocation.LEFT:
                    return Capture(
                        0,
                        ps.Bounds.Bottom - HEIGHT,
                        ps.Bounds.Width - ps.WorkingArea.Width,
                        HEIGHT);
                case TaskBarLocation.RIGHT:
                    return Capture(
                        ps.WorkingArea.Right,
                        ps.Bounds.Bottom - HEIGHT,
                        ps.Bounds.Width - ps.WorkingArea.Width,
                        HEIGHT);
                
                default:
                    return null;
            }
        }

        private static Bitmap Capture(int x, int y, int width, int height)
        {
            var bmpScreenCapture = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmpScreenCapture))
            {
                g.CopyFromScreen(x, y,
                                 0, 0,
                                 bmpScreenCapture.Size,
                                 CopyPixelOperation.SourceCopy);
            }
            return bmpScreenCapture;
        }



        enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT }

        static TaskBarLocation GetTaskBarLocation()
        {
            bool taskBarOnTopOrBottom = (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width);
            if (taskBarOnTopOrBottom)
            {
                return Screen.PrimaryScreen.WorkingArea.Top > 0 ?
                    TaskBarLocation.TOP : TaskBarLocation.BOTTOM;
            }
            else
            {
                return Screen.PrimaryScreen.WorkingArea.Left > 0 ?
                    TaskBarLocation.LEFT : TaskBarLocation.RIGHT;
            }
        }


        public static bool ContainsBitmap(Bitmap smallBmp, Bitmap bigBmp)
        {
            var pos = SearchBitmap(smallBmp, bigBmp, 0, true);
            return pos != null;
        }

        public static Rectangle? SearchBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance, bool ignoreGreen)
        {
            BitmapData smallData = 
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height), 
                       System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bigData = 
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height), 
                       System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            bool matchFound = true;

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                // Special case
                                // Bright green -- match any pixel, regardless of value
                                if (ignoreGreen
                                    && j % 3 == 0 // aligned to RGB
                                    && pSmall[0] == 0
                                    && pSmall[1] == 0xFF
                                    && pSmall[2] == 0)
                                {
                                    // move on to next pixel, ignore comparison
                                    pBig += 3;
                                    pSmall += 3;
                                    j += 2;
                                }
                                else
                                {
                                    // With tolerance: pSmall value should be between margins.
                                    int inf = pBig[0] - margin;
                                    int sup = pBig[0] + margin;
                                    if (sup < pSmall[0] || inf > pSmall[0])
                                    {
                                        matchFound = false;
                                        break;
                                    }

                                    pBig++;
                                    pSmall++;
                                }
                            }

                            if (!matchFound) break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);

            if (!matchFound) { return null; }

            return location;
        }

    }
}

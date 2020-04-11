using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitsRecogniton.Models
{
    class Binarization
    {
        private Bitmap image;
        public PixelFormat imageFormat;
        static double[] sample = new double[35];

        public Bitmap Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }

        public Binarization(Bitmap img)
        {
            this.image = this.image = img;
        }

        public void Convert()//convert to format24bpprgb
        {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed && image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                g.Dispose();
                image = bmp;
            }

            imageFormat = (image.PixelFormat == PixelFormat.Format8bppIndexed) ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;
        }

        unsafe public Bitmap ImageBinarization()//convert bitmap to white/black bitmap(0/255)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);

            BitmapData OutputData = newImage.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, imageFormat);

            BitmapData InputData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, imageFormat);

            byte* outputPointer = (byte*)OutputData.Scan0;
            byte* inputPointer = (byte*)InputData.Scan0;

            int nOffset = InputData.Stride - Image.Width * 3;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if ((inputPointer[0] + inputPointer[1] + inputPointer[2]) / 3 < 125)
                    {
                        outputPointer[0] = outputPointer[1] = outputPointer[2] = 0;
                    }
                    else
                    {
                        outputPointer[0] = outputPointer[1] = outputPointer[2] = 255;
                    }
                    inputPointer += 3; outputPointer += 3;
                }
                inputPointer += nOffset; outputPointer += nOffset;
            }
            image.UnlockBits(InputData);
            newImage.UnlockBits(OutputData);
            return newImage;
        }

        public byte[][] GetRGB(Bitmap bmp)
        {
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            IntPtr ptr = bmp_data.Scan0;
            int num_pixels = bmp.Width * bmp.Height, num_bytes = bmp_data.Stride * bmp.Height, padding = bmp_data.Stride - bmp.Width * 3, i = 0, ct = 1;
            byte[] r = new byte[num_pixels], g = new byte[num_pixels], b = new byte[num_pixels], rgb = new byte[num_bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgb, 0, num_bytes);

            for (int x = 0; x < num_bytes - 3; x += 3)
            {
                if (x == (bmp_data.Stride * ct - padding)) { x += padding; ct++; };
                r[i] = rgb[x]; g[i] = rgb[x + 1]; b[i] = rgb[x + 2]; i++;
            }
            bmp.UnlockBits(bmp_data);
            return new byte[3][] { r, g, b };
        }
        public Tuple<int, int, int, int> GetPositionOfSample(Bitmap bmp)
        {
            //Get an array containing the R,G,B components of each pixel
            var pixels = GetRGB(bmp);

            int h = bmp.Height - 1, w = bmp.Width, top = 0, bottom = h, left = bmp.Width, right = 0, white = 0;
            int tolerance = 95; // 95%

            bool prev_color = false;
            for (int i = 0; i < pixels[0].Length; i++)
            {
                int x = (i % (w)), y = (int)(Math.Floor((decimal)(i / w))), tol = 255 * tolerance / 100;
                if (pixels[0][i] >= tol && pixels[1][i] >= tol && pixels[2][i] >= tol) { white++; right = (x > right && white == 1) ? x : right; }
                else { left = (x < left && white >= 1) ? x : left; right = (x == w - 1 && white == 0) ? w - 1 : right; white = 0; }
                if (white == w) { top = (y - top < 3) ? y : top; bottom = (prev_color && x == w - 1 && y > top + 1) ? y : bottom; }
                left = (x == 0 && white == 0) ? 0 : left; bottom = (y == h && x == w - 1 && white != w && prev_color) ? h + 1 : bottom;
                if (x == w - 1) { prev_color = (white < w) ? true : false; white = 0; }
            }
            right = (right == 0) ? w : right; left = (left == w) ? 0 : left;

           
            //return cords of cut rectangle
            return Tuple.Create(left,top,right,bottom);
        }

        public void Sampling(int left, int top, int right, int bottom, Bitmap bitmap)
        {
            int sampleHeight = 7;
            int sampleWidth = 5;
            double widthRect = (right - left) / sampleWidth;
            double heightRect = (bottom - top) / sampleHeight;
            double rectangleLeft, rectangleRight, rectangleTop, rectangleBottom;          
            int idx = 0;

            for (int i = 0; i < sampleHeight; i++)
            {
                for (int j = 0; j < sampleWidth; j++)
                {
                    rectangleLeft = left + j * widthRect;
                    rectangleRight = rectangleLeft + widthRect;
                    rectangleTop = top + i * heightRect;
                    rectangleBottom = rectangleTop + heightRect;
                    if(IsRectClear(rectangleLeft, rectangleRight, rectangleTop, rectangleBottom, bitmap))
                    {
                        sample[idx++] = -0.5;
                    }
                    else
                    {
                        sample[idx++] = 0.5;
                    }
                }
            }

        }

        bool IsRectClear(double left, double right, double top, double bottom, Bitmap bitmap)
        {
            for (int i = (Int32)left; i < right; i++)
            {
                for (int j = (Int32)top; j < bottom; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    if (color.R < 50 && color.G < 50 && color.B < 50)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public string SaveSample()
        {
            Convert();
            Bitmap bitmap = ImageBinarization();
            var cords = GetPositionOfSample(bitmap);
            Sampling(cords.Item1, cords.Item2, cords.Item3, cords.Item4, bitmap);
            string show = " ";
            foreach(double x in sample)
            {
                show += x.ToString();
                show += "|";
            }
            return show;
        }


    }
}

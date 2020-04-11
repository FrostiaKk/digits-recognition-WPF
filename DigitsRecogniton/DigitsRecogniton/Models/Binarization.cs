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

    }
}

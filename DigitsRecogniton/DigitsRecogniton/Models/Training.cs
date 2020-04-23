using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DigitsRecogniton.Models
{
    static class Training
    {
		static public bool train = false;
		static public void Train(Kohonen pnet, BindableCollection<Digit> digits)
		{
			BindableCollection<Digit> Digits = digits;
			List<Digit> digitsList = Digits.ToList();
			Digit result;
			int i, j;
			double val;
			TrainingSet tset = new TrainingSet();
			for (i = 0; i < 10; i++)
			{
				result = digitsList.Find(x => x.name == i.ToString());
				for (j = 0; j < 35; j++)
				{
					if (result.sample[j] == 1)
						val = 0.5;
					else
						val = -0.5;

					tset.SetInput(i, j, val);
				}
			}
			pnet.Learn(tset);
			train = true;
		}

		static public string Recognize(object parameter, Kohonen pnet)
		{
			double norm;
			int nNeuron;

			if (!train)
			{
				return "Sieć nie została jeszcze wytrenowana.\nNaciśnij przycisk 'Trenuj sieć'";
			}

			double[] sample = new double[35];

			var size = new System.Windows.Size(450, 560);
			((UIElement)parameter).Measure(size);
			((UIElement)parameter).Arrange(new Rect(size));
			RenderTargetBitmap rtb = new RenderTargetBitmap(450, 560, 96d, 96d, PixelFormats.Default);
			rtb.Render((UIElement)parameter);
			BmpBitmapEncoder encoder = new BmpBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(rtb));

			MemoryStream stream = new MemoryStream();
			encoder.Save(stream);
			Bitmap bitmap = new Bitmap(stream);
			Binarization picture = new Binarization(bitmap);
			picture.GetSample(sample);
				nNeuron = pnet.Winner(ref sample, out norm);
			return nNeuron.ToString();
		}
	}
}

using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DigitsRecogniton.Models
{
	class SavePattern : ICommand
	{
		#region ICommand Members  
		public BindableCollection<Digit> Digits { get; set; }
		public Digit SelectedDigit;
		public SavePattern(BindableCollection<Digit> digits, Digit _selectedDigit)
		{
			Digits = digits;
			SelectedDigit = _selectedDigit;
		}
		public bool CanExecute(object parameter)
		{
			return true;
		}
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
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

			string fileName = "Samples.txt";
			string[] arrLine = File.ReadAllLines(fileName);
			int[] zeroOneArray = picture.SampleToZerosAndOnes(sample);
			SelectedDigit.SetSample(zeroOneArray);
			arrLine[Int32.Parse(SelectedDigit.name)] = SelectedDigit.SampleString();
			File.WriteAllLines(fileName, arrLine);
			//picture.ImageBinarization().Save("tescik.png", ImageFormat.Png);
			stream.Close();
		}
		#endregion


	}
}

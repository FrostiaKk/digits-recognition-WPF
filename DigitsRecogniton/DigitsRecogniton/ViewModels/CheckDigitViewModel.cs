using Caliburn.Micro;
using DigitsRecogniton.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DigitsRecogniton.ViewModels
{
    class CheckDigitViewModel : Screen
    {
		public BindableCollection<Digit> Digits { get; set; }
		public CheckDigitViewModel(BindableCollection<Digit> digits)
		{
			Digits = digits;
		}
		private ICommand _saveCanvasCommand;

		public ICommand SaveCanvasCommand
		{
			get
			{
				if (_saveCanvasCommand == null)
					_saveCanvasCommand = new SaveCanvas(Digits);
				return _saveCanvasCommand;
			}
			set { _saveCanvasCommand = value; }
		}
		class SaveCanvas : ICommand
		{
			#region ICommand Members  
			public BindableCollection<Digit> Digits { get; set; }
			public SaveCanvas(BindableCollection<Digit> digits)
			{
				Digits = digits;
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
				var size = new System.Windows.Size(100, 140);
				((UIElement)parameter).Measure(size);
				((UIElement)parameter).Arrange(new Rect(size));
				RenderTargetBitmap rtb = new RenderTargetBitmap(100, 140, 96d, 96d, PixelFormats.Default);				
				rtb.Render((UIElement)parameter);

				BmpBitmapEncoder encoder = new BmpBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(rtb));

				MemoryStream stream = new MemoryStream();
				encoder.Save(stream);
				Bitmap bitmap = new Bitmap(stream);
				Binarization pic = new Binarization(bitmap);
				double[] ddd = new double[35];
				pic.GetSample(ddd);


				//MessageBox.Show(result.SampleString());
				//MessageBox.Show(pic.SaveSample());
				//new_image.Save("tescik.png", ImageFormat.Png);
				//FileStream fs = File.Open(@"d:\test.bmp", FileMode.Create);
				//encoder.Save(fs);
				stream.Close();
			}

			

			#endregion


		}

		private ICommand _clearCanvasCommand;

		public ICommand ClearCanvasCommand
		{
			get
			{
				if (_clearCanvasCommand == null)
					_clearCanvasCommand = new ClearCanvas();
				return _clearCanvasCommand;
			}
			set { _clearCanvasCommand = value; }
		}

	}
}

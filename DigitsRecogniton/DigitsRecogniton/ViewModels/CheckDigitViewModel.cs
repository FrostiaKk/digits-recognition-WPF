using Caliburn.Micro;
using System;
using System.Collections.Generic;
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
		private ICommand _saveCanvasCommand;

		public ICommand SaveCanvasCommand
		{
			get
			{
				if (_saveCanvasCommand == null)
					_saveCanvasCommand = new SaveCanvas();
				return _saveCanvasCommand;
			}
			set { _saveCanvasCommand = value; }
		}
		class SaveCanvas : ICommand
		{
			#region ICommand Members  

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
				var size = new Size(100, 140);
				((UIElement)parameter).Measure(size);
				((UIElement)parameter).Arrange(new Rect(size));
				RenderTargetBitmap rtb = new RenderTargetBitmap(100, 140, 96d, 96d, PixelFormats.Default);
				rtb.Render((UIElement)parameter);
				BmpBitmapEncoder encoder = new BmpBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(rtb));
				FileStream fs = File.Open(@"d:\test.bmp", FileMode.Create);
				encoder.Save(fs);
				fs.Close();
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
		class ClearCanvas : ICommand
		{
			#region ICommand Members  

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
				InkCanvas newCanvas = (InkCanvas)parameter;
				newCanvas.Strokes.Clear();
				//((UIElement)parameter).
			}
			#endregion


		}
	}
}

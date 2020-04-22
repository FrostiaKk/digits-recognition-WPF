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


		public Kohonen kohonen = new Kohonen();
		public BindableCollection<Digit> Digits { get; set; }
		public CheckDigitViewModel(BindableCollection<Digit> digits)
		{
			Digits = digits;
			Training.Train(kohonen, Digits);
		}
		private ICommand _saveCanvasCommand;

		public ICommand SaveCanvasCommand
		{
			get
			{
				if (_saveCanvasCommand == null)
					_saveCanvasCommand = new SaveCanvas(Digits, kohonen);
				return _saveCanvasCommand;
			}
			set { _saveCanvasCommand = value; }
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

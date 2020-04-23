using Caliburn.Micro;
using DigitsRecogniton.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DigitsRecogniton.ViewModels
{
	class CheckDigitViewModel : Screen
	{

		private string namedigit;
		private ICommand _clearCanvasAndDigitCommand;
		private ICommand _checkDigitCommand;

		public string NameDigit
		{
			get { return namedigit; }
			set 
			{
				namedigit = value;
				NotifyOfPropertyChange(() => NameDigit);
			}
		}


		public Kohonen kohonen = new Kohonen();
		public BindableCollection<Digit> Digits { get; set; }
		public CheckDigitViewModel(BindableCollection<Digit> digits)
		{
			Digits = digits;
			Training.Train(kohonen, Digits);
		}
		

		public ICommand CheckDigitCommand
		{
			get
			{
				if (_checkDigitCommand == null)
					_checkDigitCommand = new CheckDigit(Digits, kohonen, this);
				return _checkDigitCommand;
			}
			set { _checkDigitCommand = value; }
		}
		

		

		public ICommand ClearCanvasAndDigitCommand
		{
			get
			{
				if (_clearCanvasAndDigitCommand == null)
					_clearCanvasAndDigitCommand = new ClearCanvasAndDigit(this);
				return _clearCanvasAndDigitCommand;
			}
			set { _clearCanvasAndDigitCommand = value; }
		}


	}


}

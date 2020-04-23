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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace DigitsRecogniton.ViewModels
{
    class TrainAIViewModel : Screen
    {
		private ICommand _savePatternCommand;
		private ICommand _clearCanvasCommand;
		public BindableCollection<Digit> Digits { get; set; }
		public TrainAIViewModel(BindableCollection<Digit> digits)
		{
			Digits = digits;
		}

		private Digit _selectedDigit;

		
		public Digit SelectedDigit
		{
			get { return _selectedDigit; }
			set 
			{
				_selectedDigit = value;
				NotifyOfPropertyChange(() => SelectedDigit);

				_savePatternCommand = null; //setting the field to null, will "activate" the initialization of the command
				NotifyOfPropertyChange(() => SavePatternCommand);
			}
		}

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
		
		public ICommand SavePatternCommand
		{
			get
			{
				if (_savePatternCommand == null)
					_savePatternCommand = new SavePattern(Digits, SelectedDigit);
				return _savePatternCommand;
			}
			set { _savePatternCommand = value; }
		}
		
	}
}

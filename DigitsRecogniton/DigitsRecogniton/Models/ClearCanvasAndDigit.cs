using DigitsRecogniton.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DigitsRecogniton.Models
{
    class ClearCanvasAndDigit : ICommand
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

		private CheckDigitViewModel View;
		public ClearCanvasAndDigit(CheckDigitViewModel view)
		{
			View = view;
		}
		public void Execute(object parameter)
		{
			InkCanvas newCanvas = (InkCanvas)parameter;
			newCanvas.Strokes.Clear();
			View.NameDigit = "";
		}
		#endregion
	}
}

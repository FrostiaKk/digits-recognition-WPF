using Caliburn.Micro;
using DigitsRecogniton.ViewModels;
using System;
using System.Windows.Input;

namespace DigitsRecogniton.Models
{
    class CheckDigit : ICommand
	{
		#region ICommand Members  
		public BindableCollection<Digit> Digits { get; set; }
		public CheckDigitViewModel View;

		public Kohonen kohonen { get; set; }
		public CheckDigit(BindableCollection<Digit> digits, Kohonen kohonen, CheckDigitViewModel view)
		{
			View = view;
			Digits = digits;
			this.kohonen = kohonen;
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
			View.NameDigit = Training.Recognize(parameter, kohonen);
		}
        #endregion
    }
}

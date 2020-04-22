using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DigitsRecogniton.Models
{
	class SaveCanvas : ICommand
	{
		#region ICommand Members  
		public BindableCollection<Digit> Digits { get; set; }

		public Kohonen kohonen { get; set; }
		public SaveCanvas(BindableCollection<Digit> digits, Kohonen kohonen)
		{
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
			Training.Recognize(parameter, kohonen);
		}

		#endregion
	}
}

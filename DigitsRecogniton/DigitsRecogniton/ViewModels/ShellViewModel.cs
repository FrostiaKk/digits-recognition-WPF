using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Windows.Controls;
using DigitsRecogniton.Views;
using DigitsRecogniton.Models;

namespace DigitsRecogniton.ViewModels
{
	public class ShellViewModel : Conductor<object>
	{
		public BindableCollection<Digit> digits { get; set; }
		public ShellViewModel()
		{
			SampleData data = new SampleData();
			digits = new BindableCollection<Digit>(data.SetDigits());
		}
		
		public void LoadCheckDigit()
		{
			ActivateItem(new CheckDigitViewModel(digits));
		}

		public void LoadTrainAI()
		{
			ActivateItem(new TrainAIViewModel(digits));
		}


	}
}

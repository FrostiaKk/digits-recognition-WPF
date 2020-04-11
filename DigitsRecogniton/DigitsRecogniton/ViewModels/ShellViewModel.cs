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

namespace DigitsRecogniton.ViewModels
{
	public class ShellViewModel : Conductor<object>
	{
		private string _whatDigit;

		public string WhatDigit
		{
			get { return _whatDigit; }
			set { _whatDigit = value; }
		}

		

		public void LoadCheckDigit()
		{
			ActivateItem(new CheckDigitViewModel());
		}

		public void LoadTrainAI()
		{
			ActivateItem(new TrainAIViewModel());
		}


	}
}

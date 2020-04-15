using Caliburn.Micro;
using DigitsRecogniton.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
		

		private ICommand _savePatternCommand;

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

		class SavePattern : ICommand
		{
			#region ICommand Members  
			public BindableCollection<Digit> Digits { get; set; }
			public Digit SelectedDigit;
			public SavePattern(BindableCollection<Digit> digits, Digit _selectedDigit)
			{
				Digits = digits;
				SelectedDigit = _selectedDigit;
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
				double[] sample = new double[35];
				
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
				Binarization picture = new Binarization(bitmap);
				picture.GetSample(sample);

				string fileName = "Samples.txt";
				string[] arrLine = File.ReadAllLines(fileName);
				int[] zeroOneArray = picture.SampleToZerosAndOnes(sample);
				SelectedDigit.SetSample(zeroOneArray);
				arrLine[Int32.Parse(SelectedDigit.name)] = SelectedDigit.SampleString();
				File.WriteAllLines(fileName, arrLine);
				//Digit result = digitsList.Find(x => x.name ==);
				
				stream.Close();
			}
			#endregion
			

		}
		
	}
}

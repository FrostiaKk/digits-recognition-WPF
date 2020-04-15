using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitsRecogniton.Models
{
    static class Training
    {
		static public bool train = false;
		static public void Train(Kohonen pnet, BindableCollection<Digit> digits)
		{
			BindableCollection<Digit> Digits = digits;
			List<Digit> digitsList = Digits.ToList();
			Digit result;
			int i, j;
			double val;
			TrainingSet tset = new TrainingSet();
			for (i = 0; i < 10; i++)
			{
				result = digitsList.Find(x => x.name == i.ToString());
				for (j = 0; j < 35; j++)
				{
					if (result.sample[j] == 1)
						val = 0.5;
					else
						val = -0.5;

					tset.SetInput(i, j, val);
				}
			}
			pnet.Learn(tset);
			train = true;
		}

		static public void Train(object parameter)
		{
			double norm;
			int nNeuron;

			if (!train)
			{
				MessageBox.Show("Sieć nie została jeszcze wytrenowana.\nNaciśnij przycisk 'Trenuj sieć'");
				return;
			}

		}
	}
}

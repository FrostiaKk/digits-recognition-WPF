using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitsRecogniton.Models
{
    class SampleData
    {
        public List<Digit> SetDigits()
        {
            List<Digit> output = new List<Digit>();

			int[] sampleLine = new int[35];
			int counter = 0;
			try
			{
				string line;
				System.IO.StreamReader file = new System.IO.StreamReader("Samples.txt");
				while ((line = file.ReadLine()) != null)
				{
					for (int i = 0; i < 35; i++)
					{
						sampleLine[i] = (int)Char.GetNumericValue(line[i]);
					}
					output.Add(new Digit(counter.ToString(), sampleLine));
					counter++;
				}
				file.Close();
				return output;
			}
			catch (IOException d)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(d.Message);
			}
			return output;
		}

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitsRecogniton.Models
{
    public class Digit
    {

        public readonly string name;
        public int[] sample = new int[35];
        public Digit(string name, int[] sample)
        {
            this.name = name;
            for(int i=0;i<35;i++)
            {
                this.sample[i] = sample[i];
            }
        }

        public void SetSample(int[] sample)
        {
            for (int i = 0; i < 35; i++)
            {
                this.sample[i] = sample[i];
            }
        }

        public string Name
        {
            get { return name; }
        }

        public string SampleString()
        {
            string output = "";
            for (int i = 0; i < 35; i++)
            {
                output += this.sample[i].ToString();
            }
            return output;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitsRecogniton.Models
{
    class TrainingSet
    {
        public int ntrain;
        public int nin;
        public double[,] input = new double[10,35];
        public TrainingSet()
        {
            ntrain = 10;
            nin = 35;
        }
        ~TrainingSet()
        {
        }

        public void SetInput(int set, int index, double value)
        {
            if ((set < 0) || (set >= ntrain))
                return;
            if ((index < 0) || (index >= nin))
                return;
            input[set,index] = value;
        }

    }
}

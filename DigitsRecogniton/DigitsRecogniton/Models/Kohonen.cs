using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitsRecogniton.Models
{
    class Kohonen
    {
        const int NUMBER_INPUT = 35;
        const int NUMBER_OUTPUT = 10;
        const double LEARN_RATE = 0.3;
        const double QUIT_ERROR = 0.1;
        const int RETRIES = 10000;
        const double REDUCTION = 0.99;
        int[] mapNeuron=new int[10];
        double[,] outputWeights = new double[NUMBER_OUTPUT,NUMBER_INPUT];
        double[] output = new double[NUMBER_OUTPUT];
        double s_neterr;
        public Kohonen()
        {

        }

        double VectorLength(int n,ref double[] vec)
        {
            double sum = 0.0;

            for (int i = 0; i < n; i++)
                sum += vec[i] * vec[i];
            return sum;
        }

        void NormalizeInput(ref double[] input,out double normfac)
        {
            double length;

            length = VectorLength(NUMBER_INPUT,ref input);
            if (length < 1e-30)
                length = 1e-30;

            normfac = 1.0 / Math.Sqrt(length);
        }

        double dotProduct(int n,ref double[] vec1,ref double[] vec2)
        {
            int k, m, l=0;
            double sum;

            sum = 0.0;  
            for(k = n / 4; k != 0;k--)
            {
                sum += vec1[l] * vec2[l];
                sum += vec1[l+1] * vec2[l+1];
                sum += vec1[l+2] * vec2[l+2];
                sum += vec1[l+3] * vec2[l+3];
                l+=4;
            }
            for (m = n % 4; m != 0; m--)
            {
                sum += vec1[l] * vec2[l];
                l++;
            }

            return sum;
        } // dotProduct

        void ClearWeights()
        {
            s_neterr = 1.0;
            for (int y = 0; y < NUMBER_OUTPUT; y++)
                for (int x = 0; x < NUMBER_INPUT; x++)
                    outputWeights[y,x] = 0.0;
        }

        void RandomizeWeights()
        {
            double r;

            Random rnd = new Random();
            int temp = (int)(3.464101615 / (2.0 * rnd.Next()));

            for (int y = 0; y < NUMBER_OUTPUT; y++)
            {
                for (int x = 0; x < NUMBER_INPUT; x++)
                {
                    r = (double)rnd.Next() + (double)rnd.Next() - (double)rnd.Next() - (double)rnd.Next();
                    outputWeights[y, x] = r;
                }
            }
        }

        void NormalizeWeight(ref double[] w)
        {
            int i;
            double len;

            len = VectorLength(NUMBER_INPUT,ref w);
            if (len < 1e-30)
                len = 1e-30;

            len = 1.0 / Math.Sqrt(len);
            for (i = 0; i < NUMBER_INPUT; i++)
                w[i] *= len;
        }

        void Initialize()
        {
            int i;
            double[] optr =new double[35];

            ClearWeights();
            RandomizeWeights();
            for (i = 0; i < NUMBER_OUTPUT; i++)
            {
                for (int k = 0; k < 35; k++)
                {
                    optr[k] = outputWeights[i, k];
                }
                NormalizeWeight(ref optr);
                for (int k = 0; k < 35; k++)
                {
                    outputWeights[i, k] = optr[k];
                }
            }
        } // Initialize

        void CopyWeights(Kohonen dest, Kohonen source)
        {
            dest.s_neterr = source.s_neterr;
            dest.outputWeights = source.outputWeights;
        } // CopyWeights

        void ForceWin(TrainingSet tptr,ref int[] won)
        {
            int i, tset, best, which=0;
            double dist, normfac;

            double[] dptr = new double[35];
            double[] optr = new double[35];


            dist = 1e30;
            for (tset = 0; tset < tptr.ntrain; tset++)
            {
                for (i = 0; i < 35; i++)
                {
                    dptr[i] = tptr.input[tset, i]; // Point to this case
                }
                best = Winner(ref dptr, out normfac); // Winning neuron
                for (i = 0; i < 35; i++)
                {
                    tptr.input[tset, i] = dptr[i];
                }
                if (output[best] < dist)
                {  // Far indicated by low activation
                    dist = output[best];    // Maintain record
                    which = tset;        // and which case did it
                }
            }

            for (i = 0; i < 35; i++)
            {
                dptr[i] = tptr.input[which,i];
            }
            
            best = Winner(ref dptr, out normfac);

            for (i = 0; i < 35; i++)
            {
                tptr.input[which, i] = dptr[i];
            }

            dist = -1e30;

            for (i = NUMBER_OUTPUT-1; i != -1; i--)//testing with -1
            {           // Try all neurons
                if (won[i] != 0)          // If this one won then skip it
                    continue;        // We want a non-winner
                if (output[i] > dist)
                { // High activation means similar
                    dist = output[i];   // Keep track of best
                    which = i;       // and its subscript
                }
            }

            for (i = 0; i < 35; i++)
            {
                optr[i] = outputWeights[which,i];        // Non-winner's weights
            }
            optr = dptr;
            NormalizeWeight(ref optr);
            for (i = 0; i < 35; i++)
            {
                 outputWeights[which, i] = optr[i];        // Non-winner's weights
            }
        } // ForceWin

        void AdjustWeights(double rate,ref int[] won,out double bigcorr,ref double[,] correc)
        {
            int i, j, k;
            double corr, length, f;
            double[] cptr = new double[35];
            double[] wptr = new double[35];


            bigcorr = 0.0;
            for (i = 0; i < NUMBER_OUTPUT; i++)
            {
                if (won[i] == 0)
                    continue;

                for (k = 0; k < 35; k++)
                {
                    wptr[k] = outputWeights[i,k];
                }
                for (k = 0; k < 35; k++)
                {
                    cptr[k] = correc[i,k];
                }
                

                f = (1.0 / (double)won[i]) * rate;

                length = 0.0;

                for (j = 0; j < NUMBER_INPUT; j++)
                {
                    corr = f * cptr[j];
                    wptr[j] += corr;
                    length += corr * corr;
                }

                for (k = 0; k < 35; k++)
                {
                    outputWeights[i, k] = wptr[k];
                }

                if (length > bigcorr)
                    bigcorr = length;
            }

            bigcorr = Math.Sqrt(bigcorr) / rate;

        } // AdjustWeights


        void EvaluateErrors(TrainingSet tptr, double rate,ref int[] won,out double bigerr,ref double[,] correc)
        {
            int i, best, nwts, tset;
            double normfac, length, diff;
            double[] dptr = new double[35];
            double[] wptr = new double[35];
            double[] cptr = new double[35];

            nwts = NUMBER_OUTPUT * NUMBER_INPUT;

            /*
               Zero cumulative corrections and winner counts
            */

            i = nwts;
            for (int y = 0; y < NUMBER_OUTPUT; y++)
            {
                for (int x = 0; x < NUMBER_INPUT; x++)
                {
                    correc[y,x] = 0.0;
                }
            }


            for(i=0;i<10;i++)
            {
                won[i] = 0;
            }
            bigerr = 0.0;  // Length of biggest error vector

            /*
               Cumulate the correction vector 'correc' across the epoch
            */

            for (tset = 0; tset < tptr.ntrain; tset++)
            {
                for(i=0;i<35;i++)
                {
                    dptr[i] = tptr.input[tset,i]; // Point to this case
                }              
                best = Winner(ref dptr,out normfac); // Winning neuron
                for (i = 0; i < 35; i++)
                {
                    tptr.input[tset, i] = dptr[i];
                }
                ++won[best];                   // Record this win
                for (i = 0; i < 35; i++)
                {
                    wptr[i] = outputWeights[best,i]; // Winner's weights here
                }
                for (i = 0; i < 35; i++)
                {
                    cptr[i] = correc[best,i];    // Corrections summed here
                }  
                length = 0.0;                  // Length of error vector

                for (i = 0; i < NUMBER_INPUT; i++)
                {
                    diff = dptr[i] * normfac - wptr[i]; // Input minus weight
                    length += diff * diff; // Cumulate length of error
                    cptr[i] += diff;    // just uses differences
                }                       // Loop does actual inputs
                for (i = 0; i < 35; i++)
                {
                    correc[best, i] = cptr[i];
                }

                if (length > bigerr)      // Keep track of largest error
                    bigerr = length;

            }
            bigerr = Math.Sqrt(bigerr);
        } // EvaluateErrors

        void CreateMap(TrainingSet tptr)
        {
            int tset, best, i;
            double normfac;
            double[] temp = new double[35];

            for (tset = 0; tset < tptr.ntrain; tset++)
            {
                for (i = 0; i < 35; i++)
                {
                    temp[i] = tptr.input[tset, i]; // Point to this case
                }
                best = Winner(ref temp, out normfac);//maybe back to tptr
                for (i = 0; i < 35; i++)
                {
                    tptr.input[tset, i] = temp[i]; //back to tptr
                }
                mapNeuron[best] = tset;
            }

        } // CreateMap

        public int Winner(ref double[] input,out double normfac)
        {
            int i, win = 0;
            double biggest;
            double[] optr = new double[35];


            NormalizeInput(ref input,out normfac);

            biggest = -1E30;
            for (i = 0; i < NUMBER_OUTPUT; i++)
            {
                for(int k=0;k<35;k++)
                {
                    optr[k] = outputWeights[i,k];
                }
                
                output[i] = dotProduct(NUMBER_INPUT,ref input,ref optr) * (normfac);
                for (int k = 0; k < 35; k++)
                {
                    outputWeights[i, k] = optr[k];
                }
                output[i] = 0.5 * (output[i] + 1.0);
                if (output[i] > biggest)
                {
                    biggest = output[i];
                    win = i;
                }

                // account for rounding
                if (output[i] > 1.0)
                    output[i] = 1.0;
                if (output[i] < 0.0)
                    output[i] = 0.0;
            }
            return win;
        } /* Winner */


        public bool Learn(TrainingSet tptr)
        {
            int i, tset, iter, n_retry, k;
            int[] won=new int[NUMBER_OUTPUT];
            int winners;
            double[,] correc=new double[10,35];
            double rate, best_err;
            double[] dptr=new double[35];
            double bigerr;
            double bigcorr;
            Kohonen bestnet;


            s_neterr = 1.0;
            for (tset = 0; tset < tptr.ntrain; tset++)
            {
                for(i=0;i<35;i++)
                {
                    dptr[i] = tptr.input[tset, i];
                }
                
                if (VectorLength(NUMBER_INPUT,ref dptr) < 1e-30)
                {
                    return false;
                }
                for (i = 0; i < 35; i++)
                {
                    tptr.input[tset, i] = dptr[i];
                }
            }

            bestnet = new Kohonen();
            if (bestnet == null)
                return false;

            rate = LEARN_RATE;

            Initialize();
            best_err = 1e30;


            // główna pętla ucząca
            n_retry = 0;
            for (iter = 0; ; iter++)
            {

                EvaluateErrors(tptr, rate,ref won,out bigerr,ref correc);

                s_neterr = bigerr;

                if (s_neterr < best_err)
                {
                    best_err = s_neterr;
                    CopyWeights(bestnet, this);
                }

                winners = 0;
                for (i = 0; i < NUMBER_OUTPUT; i++)
                {
                    if (won[i] != 0)
                        winners++;
                }

                if (bigerr < QUIT_ERROR)
                    break;

                if ((winners < NUMBER_OUTPUT) && (winners < tptr.ntrain))
                {
                    ForceWin(tptr, ref won);
                    continue;
                }

                

                AdjustWeights(rate,ref won,out bigcorr,ref correc);


                if (bigcorr < 1e-5)
                {
                    if (++n_retry > RETRIES)
                        break;
                    Initialize();
                    iter = -1;
                    rate = LEARN_RATE;
                    continue;
                }

                if (rate > 0.01)
                    rate *= REDUCTION;
            }

            CopyWeights(this, bestnet);
            double[] temp = new double[35];
            for (i = 0; i < NUMBER_OUTPUT; i++)
            {
                for (k = 0; k < 35; k++)
                {
                    temp[k] = outputWeights[i,k];
                }
                NormalizeWeight(ref temp);//maybe needed copy back to output
                for (k = 0; k < 35; k++)
                {
                    outputWeights[i, k] = temp[k];//back to output
                }

            }

            CreateMap(tptr);

            /*bestnet = null;
            for (i = 0; i < NUMBER_OUTPUT; i++)
            {
                delete correc[i];
            }
            delete correc;
            */
            return true;
        } /* Learn */

        int[] GetMapNeurons()
        {
            return mapNeuron;
        }

    }
}

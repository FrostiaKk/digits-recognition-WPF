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

        ~Kohonen()
            {

            }


    unsafe double VectorLength(int n, double* vec)
    {
        double sum = 0.0;

        for (int i = 0; i < n; i++)sum += vec[i] * vec[i];
        return sum;
    }

    unsafe void NormalizeInput(double* input, double* normfac)
    {
        double length;

        length = VectorLength(NUMBER_INPUT, input);
        if (length < 1e-30)length = 1e-30;

        *normfac = 1.0 / Math.Sqrt(length);
    }


    unsafe double dotProduct(int n, double* vec1, double* vec2)
    {
        int k, m;
        double sum;

        sum = 0.0;

        for(k= n / 4; k!=0;k--)
        {
            sum += *vec1 * *vec2;
            sum += *(vec1 + 1) * *(vec2 + 1);
            sum += *(vec1 + 2) * *(vec2 + 2);
            sum += *(vec1 + 3) * *(vec2 + 3);
            vec1 += 4;
            vec2 += 4;
        }

        for (m = n % 4; m != 0; m--)
        {
            sum += *vec1++ * *vec2++;
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
                outputWeights[y,x] = r;
            }
        }
    }

    unsafe void NormalizeWeight(double* w)
    {
        int i;
        double len;

        len = VectorLength(NUMBER_INPUT, w);
        if (len < 1e-30)
            len = 1e-30;

        len = 1.0 / Math.Sqrt(len);
        for (i = 0; i < NUMBER_INPUT; i++)
            w[i] *= len;
    }

    unsafe void Initialize()
    {
        int i;

        ClearWeights();
        RandomizeWeights();
            
                for (i = 0; i < NUMBER_OUTPUT; i++)
                {
                    fixed (double* pt = outputWeights)
                    {
                        NormalizeWeight(pt + i);
                    }
                }
            
    } // Initialize


    unsafe void EvaluateErrors(TrainingSet tptr, double rate, ref int[] won, double* bigerr,ref double[,] correc)
    {
        int i, best, size, nwts, tset;
        double* dptr, cptr, wptr; 
        double normfac, length, diff;



            nwts = NUMBER_OUTPUT * NUMBER_INPUT;
        size = NUMBER_INPUT;
        
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

        for( i=0;i< NUMBER_OUTPUT * sizeof(int);i++)
            {
                Buffer.SetByte(won, i, 0);
            }
        //memset(won, 0, NUMBER_OUTPUT * sizeof(int));
        *bigerr = 0.0;  // Length of biggest error vector

            /*
               Cumulate the correction vector 'correc' across the epoch
            */
            
                for (tset = 0; tset < tptr.ntrain; tset++)
                {
                    fixed (double* pt = tptr.input)
                    {
                        dptr = pt + tset; // Point to this case
                    }
                    best = Winner(dptr, &normfac); // Winning neuron
                    ++won[best];                   // Record this win
                    fixed (double* pointer = outputWeights)
                    {
                        wptr = pointer+best; // Winner's weights here
                    }
                fixed (double* pt = correc)
                {
                    cptr = pt + best;    // Corrections summed here
                }
                    length = 0.0;                  // Length of error vector

                    for (i = 0; i < NUMBER_INPUT; i++)
                    {
                        diff = dptr[i] * normfac - wptr[i]; // Input minus weight
                        length += diff * diff; // Cumulate length of error
                        cptr[i] += diff;    // just uses differences
                    }                       // Loop does actual inputs

                    if (length > *bigerr)      // Keep track of largest error
                        *bigerr = length;

                }
                *bigerr = Math.Sqrt(*bigerr);
            
    } // EvaluateErrors


    void CopyWeights(Kohonen dest, Kohonen source)
    {
        int n;

        dest.s_neterr = source.s_neterr;
        n = NUMBER_OUTPUT * NUMBER_INPUT;
        Buffer.BlockCopy(source.outputWeights, 0 , dest.outputWeights, 0, n * sizeof(double));
        } // CopyWeights


    unsafe void ForceWin(TrainingSet tptr,ref int[] won)
    {
        int i, tset, best, size, which=0;
            double* dptr, optr;
            double dist, normfac;


            size = NUMBER_INPUT;

        dist = 1e30;
        for (tset = 0; tset < tptr.ntrain; tset++)
        {
                fixed (double* pt = tptr.input)
                {
                    dptr = pt + tset; // Point to this case
                }
            best = Winner(dptr, &normfac); // Winning neuron
            if (output[best] < dist)
            {  // Far indicated by low activation
                dist = output[best];    // Maintain record
                which = tset;        // and which case did it
            }
        }
            fixed (double* pointer = tptr.input)
            {
                dptr = pointer+which;
            }
        best = Winner(dptr, &normfac);

        dist = -1e30;
        i = NUMBER_OUTPUT;
        for(i = NUMBER_OUTPUT;i!=0;i--)
        {           // Try all neurons
            if (won[i] != 0)          // If this one won then skip it
                continue;        // We want a non-winner
            if (output[i] > dist)
            { // High activation means similar
                dist = output[i];   // Keep track of best
                which = i;       // and its subscript
            }
        }
            fixed (double* pt = outputWeights)
            {
                optr = pt+which;        // Non-winner's weights
            }
        //memcpy(optr, dptr, NUMBER_INPUT * sizeof(double)); // become case
            System.Runtime.CompilerServices.Unsafe.CopyBlock(optr, dptr, NUMBER_INPUT * sizeof(double));
        NormalizeWeight(optr);
    } // ForceWin

    unsafe void AdjustWeights(double rate,ref int[] won, double* bigcorr,ref double[,] correc)
    {
        int i, j;
        double corr, length, f;
            double* cptr, wptr;


        * bigcorr = 0.0;
        for (i = 0; i < NUMBER_OUTPUT; i++)
        {
            if (won[i] == 0)
                continue;
                fixed (double* pt = outputWeights)
                {
                    wptr = pt + i;
                }
                fixed (double* pointer = correc)
                {
                    cptr = pointer + i;
                }

            f = (1.0 / (double)won[i]) * rate;

            length = 0.0;

            for (j = 0; j < NUMBER_INPUT; j++)
            {
                corr = f * cptr[j];
                wptr[j] += corr;
                length += corr * corr;
            }

            if (length > *bigcorr)
                *bigcorr = length;
        }

        *bigcorr = Math.Sqrt(*bigcorr) / rate;

    } // AdjustWeights

    unsafe void CreateMap(TrainingSet tptr)
    {
        int tset, best;
        double normfac;


        for (tset = 0; tset < tptr.ntrain; tset++)
        {
                fixed (double* pt = tptr.input)
                {
                    best = Winner(pt+tset, &normfac);
                }
            mapNeuron[best] = tset;
        }

    } // CreateMap

    ////////////////////////////////////////
    // public 
    ////////////////////////////////////////

    public unsafe int Winner(double* input, double* normfac)
    {
        int i, win = 0;
        double biggest;
        double* optr;


        NormalizeInput(input, normfac);

        biggest = -1E30;
        for (i = 0; i < NUMBER_OUTPUT; i++)
        {
                fixed (double* pt = outputWeights)
                {
                    optr = pt+i;
                }
            output[i] = dotProduct(NUMBER_INPUT, input, optr) * (*normfac);

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


    public unsafe bool Learn(TrainingSet tptr)
    {
            int i, tset, iter, n_retry, nwts;
        int[] won = new int[NUMBER_OUTPUT];
        int winners;
        double[,] correc = new double[10,35];
        double rate, best_err;
        double bigerr;
        double* dptr;
        double bigcorr;
        Kohonen bestnet = new Kohonen();


        s_neterr = 1.0;
        for (tset = 0; tset < tptr.ntrain; tset++)
        {
                fixed (double* pt = tptr.input)
                {
                    dptr = pt+tset;
                }
            if (VectorLength(NUMBER_INPUT, dptr) < 1e-30)
            {
                return false;
            }
        }

        if (bestnet == null)
            return false;

        nwts = NUMBER_INPUT * NUMBER_OUTPUT;
        rate = LEARN_RATE;

        Initialize();
        best_err = 1e30;


        // gl�wna p�tla ucz�ca
        n_retry = 0;
        for (iter = 0; ; iter++)
        {

            EvaluateErrors(tptr, rate,ref won, &bigerr,ref correc);

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
                ForceWin(tptr,ref won);
                continue;
            }

            AdjustWeights(rate,ref won, &bigcorr,ref correc);


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

            for (i = 0; i < NUMBER_OUTPUT; i++)
            {
                fixed (double* pt = outputWeights)
                {
                    NormalizeWeight(pt+i);
                }
            }

        CreateMap(tptr);

            bestnet = null;

        return true;
    } /* Learn */

    unsafe int* GetMapNeurons()
    {
            fixed (int* pt = mapNeuron)
            {
                return pt;
            }
    }
}
}

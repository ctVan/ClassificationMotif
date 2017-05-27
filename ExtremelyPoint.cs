using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    public abstract class AbstractExtremePointFinder
    {
        // array of time series
        protected float[] data;
        // length of time series
        protected long N;
        // compression rate 
        protected float R;
        public float maxR;
        public double maxDensity;
        public AbstractExtremePointFinder(float[] data, float R)
        {
            this.data = data;
            this.N = data.Length - 1;
            this.R = R;
            maxR = 0;
            maxDensity = 0;
        }
        public abstract void genExtremePoint(out long[] ExtremePointArr, out int estimatedLength);
        public abstract void estimateRatio(out long[] ExtremePointArr, out int estimatedLength);
    }

    public class ExtremePointFinder : AbstractExtremePointFinder
    {
        public ExtremePointFinder(float[] data, float R) : base(data, R)
        {

        }
        public override void estimateRatio(out long[] ExtremePointArr, out int estimatedLength)
        {
            int maxLength = 0;
            for (float j = 1.01f; j <= 2.0f; j = j + 0.05f)
            {
                R = j;
                long min, max;
                List<long> arr = new List<long>();
                long i = findFisrtTwo(out max, out min);
                if (min < max)
                {
                    arr.Add(min);
                    arr.Add(max);
                }
                else {
                    arr.Add(max);
                    arr.Add(min);
                }
                if (i < N && data[i] > data[0])
                {
                    i = findMin(i, out min);
                    arr.Add(min);
                }
                while (i < N)
                {
                    i = findMax(i, out max);
                    arr.Add(max);
                    i = findMin(i, out min);
                    arr.Add(min);
                }
                // add last point
                if (min != N)
                    arr.Add(N);
                ExtremePointArr = arr.ToArray();
                if (ExtremePointArr.LongLength < 4)
                    continue;
                double density = estimateLength(ExtremePointArr, out estimatedLength);
                if (density > maxDensity)
                {
                    maxDensity = density;
                    maxR = R;
                    maxLength = estimatedLength;
                }


            }
            ExtremePointArr = null;
            estimatedLength = maxLength;
        }



        private double estimateLength(long[] extemePointArr, out int estimatedLength)
        {
            int count1 = (int)extemePointArr.LongLength / 2;
            float len = N / count1;
            estimatedLength = (int)len;

            // count the desity
            int count2 = 0;
            for (int i = 0; i < extemePointArr.Length - 2; i = i + 2)
            {
                long ll = extemePointArr[i + 2] - extemePointArr[i];
                //  Console.WriteLine(ll);
                if ((ll / estimatedLength >= 0.9) && (ll / estimatedLength <= 1.1))
                    count2++;
            }
            double density = (double)count2 / count1;
            //          if (density >= 0.5)
            //             Console.WriteLine("R: " + R.ToString() + ", desity: " + (Math.Round(density, 2) * 100).ToString() + "%");
            return density;
        }



        public override void genExtremePoint(out long[] ExtremePointArr, out int estimatedLength)
        {
            long min, max;
            List<long> arr = new List<long>();
            arr.Add(0);
            long i = findFisrtTwo(out max, out min);
            if (i < N && data[i] > data[0])
            {
                i = findMin(i, out min);
                arr.Add(min);
            }
            while (i < N)
            {
                i = findMax(i, out max);
                arr.Add(max);
                i = findMin(i, out min);
                arr.Add(min);
            }
            // add last point
            if (min != N)
                arr.Add(N);
            ExtremePointArr = arr.ToArray();
            estimateLength(ExtremePointArr, out estimatedLength);
        }



        private long findFisrtTwo(out long max, out long min)
        {
            // find first and second significants extreme point
            long imin, imax, i;
            imin = 0;
            imax = 0;
            i = 1;
            while (i < N && (data[i] / data[imin] < R) && (data[imax] / data[i] < R))
            {
                if (data[i] < data[imin])
                    imin = i;
                if (data[i] > data[imax])
                    imax = i;
                i++;
                // for debugging
                /*             if (imin < imax)
                                 Console.WriteLine("(" + data[imin].ToString() + " " + imin + ") " + "(" + data[imax].ToString() + " " + imax + ")");
                             else
                                 Console.WriteLine("(" + data[imax].ToString() + " " + imax + ") " + "(" + data[imin].ToString() + " " + imin + ")");
                 */
                while (i < N && data[i] == 0)
                    i++;
            }
            min = imin;
            max = imax;
            return i;
        }



        private long findMin(long i, out long min)
        {
            // find first significants minimum after i-th point
            long imin = i;
            while (i < N && (data[i] / data[imin] < R))
            {
                if (data[i] < data[imin])
                    imin = i;
                i++;
                //          Console.WriteLine("(" + data[imin].ToString() + " " + imin + ")");
                while (i < N && data[i] == 0)
                    i++;
            }
            min = imin;
            return i;
        }



        private long findMax(long i, out long max)
        {
            // find first significants maximum after i-th point
            long imax = i;
            float t;
            while (i < N && ((t = data[imax] / data[i]) < R))
            {
                if (data[i] > data[imax])
                    imax = i;

                //      Console.WriteLine("(" + data[imax].ToString() + " " + data[i].ToString() + ")");
                i++;
                while (i < N && data[i] == 0)
                    i++;
            }
            max = imax;
            return i;
        }
    }
}

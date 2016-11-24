using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FindingMotifDiscord
{
    public abstract class AbstractExtremePointFinder
    {
        // array of time series
        protected float[] data;
        // length of time series
        protected long N;
        // compression rate 
        protected float R;
        public AbstractExtremePointFinder(float[] data, float R)
        {
            this.data = data;
            this.N = data.Length;
            this.R = R;
        }
        public abstract void genExtremePoint(out long[] ExtremePointArr, out int estimatedLength);
    }
    public class ExtremePointFinder : AbstractExtremePointFinder
    {
        public ExtremePointFinder(float[] data, float R) : base(data, R)
        {

        }
        private void estimateLength(long[] extemePointArr,out int estimatedLength) {
            long sumLength = 0;
            // TODO: check this if get means -> the length of time series
            for (int i = 0; i < extemePointArr.Length - 1; i++) {
                sumLength += extemePointArr[i + 1] - extemePointArr[i];
            }
            float len = sumLength / extemePointArr.Length;
            estimatedLength = (int)len;

            // count the desity
            int count = 0;
            for (int i = 0; i < extemePointArr.Length - 1; i++) {
                long ll = extemePointArr[i + 1] - extemePointArr[i];
                Console.WriteLine(ll);
                if ((ll / estimatedLength >= 0.9) && (ll / estimatedLength <= 1.1))
                    count++; 
            }
            double density = (double)count / extemePointArr.Length;
            Console.WriteLine("desity: " + (Math.Round(density,2)*100).ToString() +"%");
        }
        public override void genExtremePoint(out long[] ExtremePointArr, out int estimatedLength)
        {
            List<long> arr = new List<long>();
            arr.Add(0);
            long i = findFisrtTwo();
            if (i < N && data[i] > data[0])
            {
                i = findMin(i);
                arr.Add(i);
            }
            while (i < N) {
                i = findMax(i);
                arr.Add(i);
                i = findMin(i);
                arr.Add(i);
            }
            ExtremePointArr = arr.ToArray();
            estimateLength(ExtremePointArr, out estimatedLength);
        }
        private long findFisrtTwo()
        {
            // find first and second significants extreme point
            long imin, imax, i;
            imin = 0;
            imax = 0;
            i = 2;
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
            }
            return i;
        }
        private long findMin(long i)
        {
            // find first significants minimum after i-th point
            long imin = i;
            while (i < N && (data[i] / data[imin] < R))
            {
                if (data[i] < data[imin])
                    imin = i;
                i++;
                //          Console.WriteLine("(" + data[imin].ToString() + " " + imin + ")");
            }
            return i;
        }
        private long findMax(long i)
        {
            // find first significants minimum after i-th point
            long imax = i;
            while (i < N && (data[imax] / data[i] < R))
            {
                if (data[i] > data[imax])
                    imax = i;
                i++;
                //          Console.WriteLine("(" + data[imin].ToString() + " " + imin + ")");
            }
            return i;
        }
    }
}

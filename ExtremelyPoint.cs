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
        protected double[] data;
        // length of time series
        protected long N;
        // compression rate 
        protected double R;
        public AbstractExtremePointFinder(double[] data, double R)
        {
            this.data = data;
            this.N = data.Length;
            this.R = R;
        }
        public abstract void genExtremePoint(out long[] ExtremePointArr);
    }
    public class ExtremePointFinder : AbstractExtremePointFinder
    {
        public ExtremePointFinder(double[] data, double R) : base(data, R)
        {

        }
        public override void genExtremePoint(out long[] ExtremePointArr)
        {
            List<long> arr = new List<long>();
            arr.Add(0);
            long i = findFisrtTwo();
            if (i < N && data[i] > data[0])
            {
                i = findMin(i);
                arr.Add(i);
            }
            while (i < N)
            {
                i = findMax(i);
                arr.Add(i);
                i = findMin(i);
                arr.Add(i);
            }
            ExtremePointArr = arr.ToArray();
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

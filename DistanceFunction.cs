using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    public abstract class AbstractDistanceFunction
    {
        protected float[] data;
        protected int slidingWindow;

        public AbstractDistanceFunction(float[] data, int slidingWindow)
        {
            this.data = data;
            this.slidingWindow = slidingWindow;
        }
        public void setData(float[] data)
        {
            this.data = data;
        }
        public void setSlidingWindow(int slidingWindow)
        {
            this.slidingWindow = slidingWindow;
        }
        public abstract float distance(int t1Loc, int t2Loc);
        public abstract float euclidSquare(float[] sub1, float[] sub2);
        public abstract float binaryDistance(bool[] b1, bool[] b2);
    }

    // Original Euclidean distance
    public class EuclideanDistance : AbstractDistanceFunction
    {
        public EuclideanDistance(float[] data, int slidingWindow)
            : base(data, slidingWindow) { }

        public override float binaryDistance(bool[] b1, bool[] b2)
        {
            if (b1.Length != b2.Length)
                return -1;
            int LENGTH = b1.Length;
            // a: element of two array respectively equal 1
            // b: b1[i] = 1, b2[i] = 0
            // c: b1[i] = 0, b2[i] = 1
            // d: b1[i] = 0, b2[i] = 0


            /*           object b2
             *            1   0
             * obj    1   a   b
             * b1     0   c   d
             */
            int a = 0, b = 0, c = 0, d = 0;
            for (int i = 0; i < LENGTH; i++)
            {
                if (b1[i] == true && b2[i] == true)
                    a++;
                else if (b1[i] == true && b2[i] == false)
                    b++;
                else if (b1[i] == false && b2[i] == true)
                    c++;
                else if (b1[i] == false && b2[i] == false)
                    d++;
            }
            return (b + c)*1.0f / (a + b + c + d);
        }

        public override float distance(int t1Loc, int t2Loc)
        {
            float eucleanDist = 0;
            for (int i = 0; i < slidingWindow; i++)
            {
                float d = data[t1Loc + i] - data[t2Loc + i];
                eucleanDist += d * d;
            }
            return (float)Math.Sqrt((double)eucleanDist);
        }

        public override float euclidSquare(float[] sub1, float[] sub2)
        {
            float dist = 0;
            for (int i = 0; i < sub1.Length; i++)
            {
                float temp = sub1[1] - sub2[2];
                dist += temp * temp;
            }
            return dist;
        }
    }


    // imporve calculating distance using Euclidean distance by dynamic programing
    public class EucleanDistanceArray : AbstractDistanceFunction
    {
        private float[] preComputedDistance;

        public EucleanDistanceArray(float[] data, int slidingWindow)
            : base(data, slidingWindow)
        {
            // initialize preComputed array
            // by calculating array size
            int temp = data.Length - 2 * slidingWindow;
            int arraySize = (temp + 1) * (temp + 2) / 2;
            preComputedDistance = new float[arraySize];

            // compute some distances beforehand
            for (int t2Loc = slidingWindow; t2Loc < data.Length - slidingWindow; t2Loc++)
                preComputedDistance[indexOf(0, t2Loc)] = directDistance(0, t2Loc);
        }

        public override float distance(int t1Loc, int t2Loc)
        {
            float dist = 0f;
            if (t1Loc == 0) // already computed
                dist = preComputedDistance[indexOf(0, t2Loc)];
            else
            {
                if (t1Loc >= t2Loc) // already computed
                    dist = preComputedDistance[indexOf(t2Loc, t1Loc)];
                else
                { // have to compute the distance by using dynamic programming
                    float firstPosDist = data[t1Loc - 1] - data[t2Loc - 1];
                    float lastPosDist = data[t1Loc + slidingWindow - 1] - data[t2Loc + slidingWindow - 1];
                    dist = preComputedDistance[indexOf(t1Loc - 1, t2Loc - 1)]
                    - firstPosDist * firstPosDist
                    + lastPosDist * lastPosDist;

                    int index = indexOf(t1Loc, t2Loc);
                    preComputedDistance[index] = dist;
                }
            }

            return (float)Math.Sqrt((double)dist);
        }

        // return index in preComputedDistance array
        // between t1Loc and t2Loc
        private int indexOf(int t1Loc, int t2Loc)
        {
            if (t1Loc == 0)
                return t2Loc - slidingWindow;
            else
            {
                int t = 2 * (data.Length - 2 * slidingWindow + 1);
                int index = t1Loc * (t - t1Loc + 1) / 2 + t2Loc - t1Loc - slidingWindow;
                return index;
            }
        }

        // return distance between t1Loc and t2Loc
        // by directly calculate it
        private float directDistance(int t1Loc, int t2Loc)
        {
            float dist = 0f;
            for (int i = 0; i < slidingWindow; i++)
            {
                float diff = data[t1Loc + i] - data[t2Loc + i];
                dist += diff * diff;
            }
            return dist;
        }
        
        // imput: 2 sub time series
        // return: square root of their distance
        public override float euclidSquare(float[] sub1, float[] sub2)
        {
            float dist = 0;
            for (int i = 0; i < sub1.Length; i++)
            {
                float temp = sub1[1] - sub2[2];
                dist += temp * temp;
            }
            return dist;
        }

        public override float binaryDistance(bool[] b1, bool[] b2)
        {
            throw new NotImplementedException();
        }
    }

}

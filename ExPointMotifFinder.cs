using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    // Motif finder using Extreme Point

    public class ExPointMotifFinder : AbstractMotifFinder
    {
        public struct Point
        {
            public int location;
            public double distance;
            public Point(int location, double distance)
            {
                this.location = location;
                this.distance = distance;
            }
        }

        public ExPointMotifFinder(float[] data, int slidingWindow, float R, AbstractDistanceFunction disFunc)
            : base(data, slidingWindow, R, disFunc)
        {

        }

        public override void findMotif(out int motifLoc, out int[] motifMatches, out long[] ExtremePointArr)
        {
            double bestSoFar = Double.MaxValue;
            int motifLocation1 = -1;
            int[] motifLocation2 = { -1 };

            int isRatio = 0;
            float r = 1.37f;
            int lengthMotif = 400;
            AbstractExtremePointFinder EPF = new ExtremePointFinder(data, r);

            //        long[] ExtremePointArr;
            if (isRatio == 1)
                EPF.estimateRatio(out ExtremePointArr, out lengthMotif);
            else
                EPF.genExtremePoint(out ExtremePointArr, out lengthMotif);

            Console.WriteLine("Sliding window: " + lengthMotif.ToString() + ", R" + EPF.maxR.ToString());

            // TODO: calculate standard length of motif cadidates
            Homothety homothey = new Homothety(lengthMotif);

            // create new data array
            int countSubseuquence = 0;
            float[][] dataArr = new float[ExtremePointArr.Length/2][];
            for (int i = 0; i < ExtremePointArr.Length - 1; i = i + 2)
            {
                long begin = ExtremePointArr[i];
                long end;
                if (i == ExtremePointArr.Length - 2)
                    end = ExtremePointArr[ExtremePointArr.Length - 1];
                else
                    end = ExtremePointArr[i + 2];

               // Console.WriteLine("begin: " + begin.ToString() + ", end: " + end.ToString());
                // copy subsequence to new array
                float[] inArr;
                inArr = new float[end - begin];
                for (long j = begin; j < end; j++)
                    inArr[j - begin] = data[j];

                dataArr[countSubseuquence] = homothey.transform(inArr);
                countSubseuquence++;
            }

            // calculate distance
            Point[] distances = new Point[dataArr.Length];
            // chose 0 as a reference point
            int refObj = 0;

            for (int i = 1; i < dataArr.Length; i++)
            {
                Point dist;
                dist.distance = distFunc.euclidSquare(dataArr[refObj], dataArr[i]);
                // we'll convert back to actual index later
                dist.location = i;

                // add to the distances
                distances[i - 1] = dist;

                // defenitely not trivial match
                if (dist.distance < bestSoFar)
                {
                    // update bestsofar
                    bestSoFar = dist.distance;
                    motifLocation2[0] = i;
                }
            }
            // Sort the distances ascending
            Array.Sort(distances, delegate (Point x, Point y)
            {
                return x.distance.CompareTo(y.distance);
            });


            // Begin finding motif pair
            int offset = 0;
            bool abandon = false;
            while (!abandon)
            {
                ++offset;
                abandon = true;
                for (int i = 0; i < distances.Length - 1 - offset; i++)
                {
                    Point d1 = distances[i];
                    Point d2 = distances[i + offset];

                    // calculate the distance between them
                    if (d2.distance - d1.distance < bestSoFar)
                    {
                        abandon = false;
                        double d = distFunc.distance(d1.location, d2.location);

                        if (d < bestSoFar)
                        {
                            bestSoFar = d;
                            motifLocation1 = d1.location;
                            motifLocation2[0] = d2.location;
                        }
                    }

                }
            }
            // Return to the caller
            motifLoc = motifLocation1;
            //          motifLocation2[0] = (int)ExtremePointArr[motifLocation2[0] * 2];
            motifMatches = motifLocation2;
            slidingWindow = 0;
        }

        public override void findMotif(out int motifLoc, out int[] motifMatches)
        {
            throw new NotImplementedException();
        }
    }
}

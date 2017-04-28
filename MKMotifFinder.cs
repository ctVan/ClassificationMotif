using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    public class MKMotifFinder : AbstractMotifFinder
    {
        // This class uses for save location of 1 subsequences and
        // its distance with the reference one
        public struct Point
        {
            public int location;
            public float distance;
            public Point(int location, float distance)
            {
                this.location = location;
                this.distance = distance;
            }
        }

        public MKMotifFinder(float[] data, int slidingWindow, float R, AbstractDistanceFunction distFunc)
            : base(data, slidingWindow, R, distFunc)
        {

        }

        public override float findMotif(out int motifLoc, out int[] motifMatches)
        {
            float bestSoFar = float.MaxValue;
            int motifLocation1 = -1;
            int[] motifLocation2 = { -1 };

            // Choose a random reference point
            // For simplicity, the reference point will be 0
            int len = data.Length;
            int m = len - slidingWindow + 1;
            // for testing, we just just default reference location, in theory, it's random
            int refLocation = 0;

            // Pre calculate the distance between time series subsequences and the reference point
            motifLocation1 = refLocation;
            Point[] points = new Point[m];

            for (int i = 1; i < m; ++i)
            {
                Point dist;
                dist.distance = distFunc.distance(refLocation, i);
                dist.location = i;

                // add to the distances
                points[i - 1] = dist;

                // if the calculated distance is less than best so far
                if (i >= slidingWindow)
                {
                    if (dist.distance < bestSoFar)
                    {
                        // update bestSoFar
                        bestSoFar = dist.distance;
                        motifLocation2[0] = i;
                    }
                }
            }

            // Sort the distances ascending
            Array.Sort(points, delegate (Point x, Point y)
            {
                return x.distance.CompareTo(y.distance);
            });

            // find two susequence have the minimum distance (motif as concept of MK)
            findMotifMK(m, points, bestSoFar, ref motifLocation1, ref motifLocation2);
  //          Console.WriteLine("value: " + distFunc.distance(motifLocation1, motifLocation2[0]));
            // Return to the caller
            motifLoc = motifLocation1;
            motifMatches = motifLocation2;
            return (float)bestSoFar;
        }

        public void findMotifMK(int m, Point[] distances, double bestSoFar, ref int motifLocation1, ref int[] motifLocation2)
        {
            // Begin finding motif pair
            int offset = 0;
            bool abandon = false;
            while (!abandon)
            {
                ++offset;
                abandon = true;
                for (int i = 0; i < m - offset - 1; ++i)
                {
                    Point d1 = distances[i];
                    Point d2 = distances[i + offset];

                    // ignore time series pair whose are in sliding window
                    if (Math.Abs(d1.location - d2.location) < slidingWindow)
                        continue;

                    // if not, calculate the distance between them
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
        } // end finding motif MK

        public override void findMotif(out int motifLoc, out int[] motifMatches, out long[] ExtremePointArr, int isRatio)
        {
            throw new NotImplementedException();
        }

        public override void estimateSlidingWindow(out int lenMotif)
        {
            throw new NotImplementedException();
        }
    }// end class MKmotif Finder
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace FindingMotifDiscord
{
    public abstract class AbstractMotifFinder
    {
        protected int slidingWindow;
        protected float R;
        protected float[] data;
        protected AbstractDistanceFunction distFunc;

        public struct Distance
        {
            public int location;
            public double distance;
            public Distance(int location, double distance)
            {
                this.location = location;
                this.distance = distance;
            }
        }

        public AbstractMotifFinder(float[] data, int slidingWindow, float R, AbstractDistanceFunction distFunc)
        {
            this.data = data;
            this.slidingWindow = slidingWindow;
            this.R = R;
            this.distFunc = distFunc;
        }

        public abstract void findMotif(out int motifLoc, out int[] motifMatches);
    }

    public class MotifFinder : AbstractMotifFinder
    {
        public MotifFinder(float[] data, int slidingWindow, float R, AbstractDistanceFunction distFunc)
            : base(data, slidingWindow, R, distFunc)
        {

        }

        public override void findMotif(out int motifLoc, out int[] motifMatches)
        {
            int bestMotifCnt = -1;
            motifLoc = -1;
            List<int> motifMatchesList = null;

            for (int i = 0; i < data.Length - slidingWindow; i++)
            {
                int count = 0;
                List<int> pointers = new List<int>();
                for (int j = 0; j < data.Length - slidingWindow; j++)
                {
                    if (Math.Abs(i - j) >= slidingWindow)
                    {
                        if (distFunc.distance(i, j) < R)
                        {
                            count++;
                            pointers.Add(j);
                        }
                    }
                }

                if (count > bestMotifCnt)
                {
                    bestMotifCnt = count;
                    motifLoc = i;
                    motifMatchesList = pointers;
                }
            }

            // print matrix
            //distFunc.printMatrix();

            // return
            motifMatches = motifMatchesList.ToArray();
        }
    }

    public class MKAlgorithm : AbstractMotifFinder
    {
        // using for shared resources between several threads
        private Object lockObj = new Object();
        private ReaderWriterLock rwl = new ReaderWriterLock();
        // timeout for aquire rwl in milisecond
        int timeout = 10000;
        const int THREAD_COUNT = 4;


        public MKAlgorithm(float[] data, int slidingWindow, float R)
            : base(data, slidingWindow, R, new EucleanDistance(data, slidingWindow))
        {

        }
        public void calRefDistance(int begin,
                                  int end,
                                  int refLocation,
                                  ref double bestSoFar,
                                  ref int motifLoc2,
                                  ref Distance[] distances)
        {
            Console.WriteLine("thread: " + Thread.CurrentThread.Name + " " + begin + " " + end);
            for (int i = begin; i < end; i++)
            {

                // if the calculated distance is less than best so far
                if (i >= slidingWindow)
                {
                    double distance;
                    int location;
                    distance = distFunc.distance(refLocation, i);
                    location = i;
                    // just use lock, can be updated using read write lock
                    // Update: repplace lock byreadwritelock
                    try
                    {
                        rwl.AcquireReaderLock(timeout);

                        // add to the distances
                        distances[i - 1].distance = distance;
                        distances[i - 1].location = location;

                        if (distance < bestSoFar)
                        {
                            LockCookie lc = rwl.UpgradeToWriterLock(timeout);
                            try
                            {
                                // update bestSoFar
                                bestSoFar = distance;
                                motifLoc2 = i;
                            }
                            finally
                            {
                                rwl.DowngradeFromWriterLock(ref lc);
                            }
                        }
                    }
                    finally
                    {
                        rwl.ReleaseReaderLock();
                    }
                }
            }
        }
        public override void findMotif(out int motifLoc, out int[] motifMatches)
        {
            double bestSoFar = Double.MaxValue;
            int motifLocation1 = -1;
            int[] motifLocation2 = { -1 };

            // Choose a random reference point
            // For simplicity, the reference point will be 0
            int len = data.Length;
            int m = len - slidingWindow + 1;
            int refLocation = 0;

            // Pre calculate the distance between time series subsequences and the reference point
            //List<Distance> distances = new List<Distance> ();
            motifLocation1 = refLocation;
            Distance[] distances = new Distance[m];

            /*          Pause implement using multi thread
                        // initialize array of distances
                        for (int i = 0; i < m; i++)
                        {
                            distances[i] = new Distance();
                        }

                        // Calculate using multi-threads
                        Thread[] th = new Thread[THREAD_COUNT];
                        for (int i = 0; i < THREAD_COUNT; i++)
                        {
                            int begin, end;
                            if (i == 0)
                                begin = 1;
                            else
                                begin = i * len / THREAD_COUNT;
                            if (i == THREAD_COUNT - 1)
                                end = m;
                            else
                                end = (i + 1) * len / THREAD_COUNT - 1;
                            Console.WriteLine("cap thread: " + i + " " + begin + " " + end);
                            ThreadStart starter = () => calRefDistance(begin, end, refLocation, ref bestSoFar, ref motifLocation2[0], ref distances);

                            th[i] = new Thread(starter);
                            th[i].Name = i.ToString();
                            th[i].Start();
                        }
                        foreach (Thread th_ in th)
                        {
                            th_.Join();
                        }
            */
            // Calculate using single thread
            for (int i = 1; i < m; ++i)
            {
                Distance dist;
                dist.distance = distFunc.distance(refLocation, i);
                dist.location = i;

                // add to the distances
                distances[i - 1] = dist;

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
            //distances.Sort ((x, y) => x.distance.CompareTo (y.distance));
            Array.Sort(distances, delegate (Distance x, Distance y)
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
                for (int i = 0; i < m - offset - 1; ++i)
                {
                    Distance d1 = distances[i];
                    Distance d2 = distances[i + offset];

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
            Console.WriteLine("value: " + distFunc.distance(motifLocation1, motifLocation2[0]));
            // Return to the caller
            motifLoc = motifLocation1;
            motifMatches = motifLocation2;
        }


    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Motif finder using Extreme Point

    public class ExPointMotifFinder : AbstractMotifFinder
    {
        public ExPointMotifFinder(float[] data, int slidingWindow, float R)
            : base(data, slidingWindow, R, new EucleanDistance(data, slidingWindow))
        {

        }
        // TODO: change this
        private float __distance(float[] sub1, float[] sub2)
        {
            float dist = 0;
            for (int i = 0; i < sub1.Length; i++)
            {
                float temp = sub1[1] - sub2[2];
                dist += temp * temp;
            }
            return dist;
        }
        public override void findMotif(out int motifLoc, out int[] motifMatches)
        {
            double bestSoFar = Double.MaxValue;
            int motifLocation1 = -1;
            int[] motifLocation2 = { -1 };

            float r = 1.5f;
            int lengthMotif = 0;
            AbstractExtremePointFinder EPF = new ExtremePointFinder(data, r);
            long[] ExtremePointArr;
            EPF.genExtremePoint(out ExtremePointArr, out lengthMotif);
            // TODO: calculate standard length of motif cadidates
            Homothety homothey = new Homothety(lengthMotif);

            // create new data array
            float[][] dataArr = new float[ExtremePointArr.Length - 1][];
            for (int i = 0; i < ExtremePointArr.Length - 1; i++)
            {
                long begin = ExtremePointArr[i];
                begin = (begin == 0) ? begin : begin + 1;
                long end = ExtremePointArr[i + 1];
                // copy subsequence to new array
                float[] inArr;
                if (begin ==0)
                    inArr = new float[end - begin+1];
                else
                    inArr = new float[end - begin];
                for (long j = begin; j <= end; j++)
                {
                    inArr[j-begin] = data[j];
                }
                dataArr[i] = homothey.transform(inArr);
            }

            // calculate distance
            Distance[] distances = new Distance[dataArr.Length];
            // chose 0 as a reference point
            int refObj = 0;

            for (int i = 1; i < dataArr.Length; i++)
            {
                Distance dist;
                dist.distance = __distance(dataArr[refObj], dataArr[i]);
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
            Array.Sort(distances, delegate (Distance x, Distance y)
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
                for (int i = 0; i < distances.Length; i++)
                {
                    Distance d1 = distances[i];
                    Distance d2 = distances[i + offset];

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
            motifMatches = motifLocation2;
        }
    }
}


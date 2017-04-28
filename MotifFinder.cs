using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    public abstract class AbstractMotifFinder
    {
        protected int slidingWindow;
        protected float R;
        protected float[] data;
        protected AbstractDistanceFunction distFunc;

        public AbstractMotifFinder(float[] data, int slidingWindow, float R, AbstractDistanceFunction distFunc)
        {
            this.data = data;
            this.slidingWindow = slidingWindow;
            this.R = R;
            this.distFunc = distFunc;
        }
        public void setData(float[] data) {
            this.data = data;
            distFunc.setData(data);
        }
        public void setSlidingWindow(int slidingWindow) {
            this.slidingWindow = slidingWindow;
            distFunc.setSlidingWindow(slidingWindow);
        }
        public abstract float findMotif(out int motifLoc, out int[] motifMatches);
        public abstract void findMotif(out int motifLoc, out int[] motifMatches, out long[] ExtremePointArr, int isRatio);
        public abstract void estimateSlidingWindow(out int lenMotif);
    }

    public class MotifFinder : AbstractMotifFinder
    {
        public MotifFinder(float[] data, int slidingWindow, float R, AbstractDistanceFunction distFunc)
            : base(data, slidingWindow, R, distFunc)
        {

        }

        public override void estimateSlidingWindow(out int lenMotif)
        {
            throw new NotImplementedException();
        }

        public override float findMotif(out int motifLoc, out int[] motifMatches)
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
            // return
            motifMatches = motifMatchesList.ToArray();
            return 0;
        }

        public override void findMotif(out int motifLoc, out int[] motifMatches, out long[] ExtremePointArr, int isRatio)
        {
            throw new NotImplementedException();
        }
    }
}

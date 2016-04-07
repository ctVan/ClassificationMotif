using System;
using System.Collections.Generic;

namespace FindingMotifDiscord
{
	public abstract class AbstractMotifFinder
	{
		protected int slidingWindow;
		protected float R;
		protected float[] data;
		protected AbstractDistanceFunction distFunc;

		public AbstractMotifFinder(float[] data, int slidingWindow, float R)
		{
			this.data = data;
			this.slidingWindow = slidingWindow;
			this.R = R;
			distFunc = null;
		}

		public abstract void findMotif(out int motifLoc, out int[] motifMatches);
	}

	public class MotifFinder : AbstractMotifFinder
	{
		public MotifFinder (float[] data, int slidingWindow, float R)
			: base (data, slidingWindow, R)
		{
			distFunc = new EucleanDistanceArray (data, slidingWindow);
		}

		public override void findMotif(out int motifLoc, out int[] motifMatches)
		{
			int bestMotifCnt = -1;
			motifLoc = -1;
			List<int> motifMatchesList = null;

			for (int i = 0; i < data.Length - slidingWindow; i++) {
				int count = 0;
				List<int> pointers = new List<int>();
				for (int j = 0; j < data.Length - slidingWindow; j++) {
					if (Math.Abs (i - j) >= slidingWindow) {
						if (distFunc.distance (i, j) < R) {
							count++;
							pointers.Add (j);
						}
					}
				}

				if (count > bestMotifCnt) {
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
}


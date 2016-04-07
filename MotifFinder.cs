using System;
using System.Collections.Generic;

namespace FindingMotifDiscord
{
	public class MotifFinder
	{
		private const int slidingWindow = 5;
		private const float R = 0.1f;
		AbstractDistanceFunction distFunc;
		private float[] data;

		public MotifFinder (float[] data)
		{
			this.data = data;
			distFunc = new ImprovedEucleanDistance (data, slidingWindow);
		}

		public void findMotif(out int motifLoc, out int[] motifMatches)
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


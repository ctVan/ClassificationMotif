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
		public MotifFinder (float[] data, int slidingWindow, float R, AbstractDistanceFunction distFunc)
			: base (data, slidingWindow, R, distFunc)
		{
			
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

	public class MKAlgorithm : AbstractMotifFinder
	{
		private struct Distance
		{
			public int location;
			public double distance;
			public Distance(int location, double distance)
			{
				this.location = location;
				this.distance = distance;
			}
		}

		public MKAlgorithm(float[] data, int slidingWindow, float R)
			: base(data, slidingWindow, R, new EucleanDistance(data, slidingWindow))
		{

		}

		public override void findMotif(out int motifLoc, out int[] motifMatches)
		{
			double bestSoFar = Double.MaxValue;
			int motifLocation1 = -1;
			int[] motifLocation2 = {-1};

			// Choose a random reference point
			// For simplicity, the reference point will be 0
			int m = data.Length - slidingWindow + 1;
			int refLocation = 0;

			// Pre calculate the distance between time series subsequences and the reference point
			//List<Distance> distances = new List<Distance> ();
			Distance[] distances = new Distance[m];
			for (int i = 1; i < m; ++i) {
				Distance dist;
				dist.distance = distFunc.distance (refLocation, i);
				dist.location = i;

				// add to the distances
				distances[i - 1] = dist;

				// if the calculated distance is less than best so far
				if (i >= slidingWindow) {
					if (dist.distance < bestSoFar)
					{
						// update bestSoFar
						bestSoFar = dist.distance;
						motifLocation1 = refLocation;
						motifLocation2[0] = i;
					}
				}
			}

			// Sort the distances ascending
			//distances.Sort ((x, y) => x.distance.CompareTo (y.distance));
			Array.Sort(distances, delegate (Distance x, Distance y) {
				return x.distance.CompareTo(y.distance);
			});

			// Begin finding motif pair
			int offset = 0;
			bool abandon = false;
			while (!abandon) {
				++offset;
				abandon = true;
				for (int i = 0; i < m - offset - 1; ++i) {
					Distance d1 = distances [i];
					Distance d2 = distances [i + offset];

					// ignore time series pair whose are in sliding window
					if (Math.Abs(d1.location - d2.location) < slidingWindow)
						continue;

					// if not, calculate the distance between them
					if (d2.distance - d1.distance < bestSoFar) {
						abandon = false;
						double d = distFunc.distance (d1.location, d2.location);

						if (d < bestSoFar) {
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


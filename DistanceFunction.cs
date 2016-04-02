using System;

namespace FindingMotifDiscord
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

		public abstract float distance(int t1Loc, int t2Loc);

		//public virtual void printMatrix() {}
	}

	public class EucleanDistance : AbstractDistanceFunction
	{
		public EucleanDistance (float[] data, int slidingWindow)
			: base (data, slidingWindow)
		{
		}

		public override float distance(int t1Loc, int t2Loc)
		{
			float eucleanDist = 0;
			for (int i = 0; i < slidingWindow; i++) {
				float d = data [t1Loc + i] - data [t2Loc + i];
				eucleanDist += d * d;
			}
			return (float)Math.Sqrt ((double)eucleanDist);
		}
	}

	public class ImprovedEucleanDistance : AbstractDistanceFunction
	{
		private float[][] preComputedDistance;

		public ImprovedEucleanDistance(float[] data, int slidingWindow)
			: base(data, slidingWindow)
		{
			// generate computed distance matrix
			int preComputedMatrixSize = data.Length - slidingWindow;
			preComputedDistance = new float[preComputedMatrixSize][];
			for (int i = 0; i < preComputedMatrixSize; i++)
				preComputedDistance [i] = new float[i + 1];

			// initialize diagnal
			for (int i = 0; i < preComputedMatrixSize; i++)
				preComputedDistance [i] [i] = 0;

			// compute first row distance
			for (int i = 1; i < preComputedMatrixSize; i++)
				preComputedDistance [i] [0] = __distance (0, i);

		}

		// calculate distance without dynamic quy hoach
		private float __distance(int t1Loc, int t2Loc)
		{
			float dist = 0;
			for (int i = 0; i < slidingWindow; i++) {
				float temp = data [t1Loc + i] - data [t2Loc + i];
				dist += temp * temp;
			}
			return dist;
		}

		// calculate distance with dynamic quy hoach
		public override float distance(int t1Loc, int t2Loc)
		{
			float dist = 0;
			if (t1Loc >= t2Loc) // distance already computed
				dist = preComputedDistance[t1Loc][t2Loc];
			else {
				if (t1Loc == 0) // distance computed
					dist = preComputedDistance [t2Loc] [0];
				else {
					// need to compute distance
					float firstPosDist = data[t1Loc - 1] - data[t2Loc - 1];
					float lastPosDist = data [t1Loc + slidingWindow - 1] - data [t2Loc + slidingWindow - 1];
					dist = preComputedDistance [t2Loc - 1] [t1Loc - 1]
					- firstPosDist * firstPosDist
					+ lastPosDist * lastPosDist;

					preComputedDistance [t2Loc] [t1Loc] = dist;
				}
			}
			return (float)Math.Sqrt ((double)dist);
		}

		/*
		public override void printMatrix()
		{
			foreach (float[] row in preComputedDistance) {
				foreach (float col in row) {
					if (col != 0)
						System.Console.Write ("X ");
					else
						System.Console.Write (col.ToString () + " ");
				}
				System.Console.WriteLine ();
			}
		}*/
	}
}


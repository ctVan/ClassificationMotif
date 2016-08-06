using System;
using System.Collections.Generic;

namespace FindingMotifDiscord
{
	public abstract class FindingSlidingWindow
	{
		public FindingSlidingWindow ()
		{
		}

		public abstract int findingSlidingWindow(float[] data);
	}

	public class AverageSlidingWindow : FindingSlidingWindow
	{
		public AverageSlidingWindow()
		{

		}

		public override int findingSlidingWindow(float[] data)
		{
			// find important extreme points
			List<int> extremePoints = findImportantExtremePoint (data);

			// forecast the sliding window base on averaging
			return data.Length / extremePoints.Count;
		}

		private List<int> findImportantExtremePoint(float[] data)
		{
			List<int> extremePoints = new List<int> ();

			bool isIncreasing;
			if (data [0] <= data [1])
				isIncreasing = true;
			else
				isIncreasing = false;

			int i = 2;
			bool justIncrease;
			do {
				// determine if the value is just increase
				if (data[i - 1] <= data[i])
					justIncrease = true;
				else
					justIncrease = false;

				// determine if we are still inscreasing/decreasing
				if (isIncreasing ^ justIncrease) {
					// no, we are not
					extremePoints.Add(i - 1);

					// update the trend
					isIncreasing = justIncrease;
				}

				// increment the counting
				++i;
			} while (i < data.Length);

			return extremePoints;
		}
	}
}


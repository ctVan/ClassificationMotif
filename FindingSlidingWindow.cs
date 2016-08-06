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
		const int segmentsPerSlidingWindow = 3;

		public AverageSlidingWindow()
		{

		}

		public override int findingSlidingWindow(float[] data)
		{
			// find important extreme points
			List<int> extremePoints = findImportantExtremePoint (data);

			// calculate the total length of segmentPerSlidingWindow in the time series
			int totalLength = 0;
			int prevExtremePointLoc = 0;
			for (int i = 0; i < extremePoints.Count; ++i) {
				int times = 0;
				if (i < segmentsPerSlidingWindow)
					times = (i + 1);
				else if (i <= extremePoints.Count - segmentsPerSlidingWindow)
					times = segmentsPerSlidingWindow;
				else
					times = (extremePoints.Count - i);

				totalLength += times * (extremePoints[i] - prevExtremePointLoc);
				prevExtremePointLoc = extremePoints [i];
			}

			// number of sub time series if we have segmentPerSlidingWindow
			int numOfSubTimeSeries = extremePoints.Count - segmentsPerSlidingWindow;

			// calculate the sliding window based on averaging
			return totalLength / numOfSubTimeSeries;
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


using System;

namespace FindingMotifDiscord
{
	public class Homothety
	{
		private int standardLength;

		public Homothety (int standardLength)
		{
			this.standardLength = standardLength;
		}

		public float[] transform(float[] timeSeries)
		{
			if (timeSeries.Length == standardLength)
				return (float[])timeSeries.Clone ();
			else {
				// find the min and max in time series
				float yMin = findMinValue (timeSeries);
				float yMax = findMaxValue (timeSeries);

				// find the centre of time series
				float xCenter = timeSeries.Length * 1.0f / 2;
				float yCenter = (yMin + yMax) / 2;

				// transform the time series to predefined length
				float k = standardLength * 1.0f / timeSeries.Length;
				float[] result;
				backwardTransform (out result, timeSeries, xCenter, yCenter, k);
				return result;
			}
		}

		private void backwardTransform(out float[] destination, float[] timeSeries, float xCenter, float yCenter, float k)
		{
			destination = new float[standardLength];
			float start = -k * xCenter + xCenter;
			for (int i = 0; i < standardLength; ++i) {
				float xOriginalIndex = (i + start - xCenter) / k + xCenter;
				int xOriginalIndexRounded = (int)xOriginalIndex;
				destination [i] = bilinearInterpolate (
					timeSeries [xOriginalIndexRounded], timeSeries [xOriginalIndexRounded + 1], xOriginalIndexRounded, xOriginalIndex);
			}
		}

		private float bilinearInterpolate(float leftValue, float rightValue, float leftIndex, float index)
		{
			float rightPercentage = index - leftIndex;
			float leftPercentage = 1.0f - rightPercentage;

			return leftValue * leftPercentage + rightValue * rightPercentage;
		}

		private static float findMinValue(float[] timeSeries) 
		{
			float minValue = float.MaxValue;
			foreach (float value in timeSeries) {
				if (minValue > value)
					minValue = value;
			}

			return minValue;
		}

		private static float findMaxValue(float[] timeSeries)
		{
			float maxValue = float.MinValue;
			foreach (float value in timeSeries) {
				if (maxValue < value)
					maxValue = value;
			}

			return maxValue;
		}
	}
}


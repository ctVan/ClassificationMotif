﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    public class Homothety
    {
        private int standardLength;

        public Homothety(int standardLength)
        {
            this.standardLength = standardLength;
        }

        public float[] transform(float[] timeSeries)
        {
            if (timeSeries.Length == standardLength)
                return (float[])timeSeries.Clone();
            else
            {
                // find the centre of time series
                float xCenter = (timeSeries.Length - 2) * 1.0f / 2;

                // transform the time series to predefined length
                // only transform timeSerires.Length - 1 to new length
                // because there isn't enough data length to map to new time series
                float k = standardLength * 1.0f / (timeSeries.Length - 1);
                return backwardTransform(timeSeries, xCenter, k);
            }
        }

        private float[] backwardTransform(float[] timeSeries, float xCenter, float k)
        {
            float[] destination = new float[standardLength];
            float start = -k * xCenter + xCenter;
            for (int i = 0; i < standardLength; ++i)
            {
                float xOriginalIndex = (i + start - xCenter) / k + xCenter;
                int xOriginalIndexRounded = (int)xOriginalIndex;
                destination[i] = bilinearInterpolate(
                    timeSeries[xOriginalIndexRounded], timeSeries[xOriginalIndexRounded + 1], xOriginalIndexRounded, xOriginalIndex);
            }

            return destination;
        }

        private float bilinearInterpolate(float leftValue, float rightValue, float leftIndex, float index)
        {
            float rightPercentage = index - leftIndex;
            float leftPercentage = 1.0f - rightPercentage;

            return leftValue * leftPercentage + rightValue * rightPercentage;
        }
    }
}

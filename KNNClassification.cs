using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{

    class KNNClassification
    {
        AbstractDistanceFunction disFunc;
        BinaryData[] TimeseriesArrBin;
        RealData[] realData;
        public KNNClassification(BinaryData[] dataArr, AbstractDistanceFunction _disFunc)
        {
            this.TimeseriesArrBin = dataArr;
            this.disFunc = _disFunc;
        }

        public KNNClassification(RealData[] dataArr, AbstractDistanceFunction _disFunc)
        {
            this.realData = dataArr;
            this.disFunc = _disFunc;
        }

        public void classify(BinaryData newTimeseries, out string nhan)
        {
            // find the best match with this 
            List<int> cluster = new List<int>();
            // find best match and not in list "cluster"
            int bestIndex = 0;
            float bestsoFar = float.MaxValue;
            float distance = 0;

            for (int i = 0; i < TimeseriesArrBin.Length; i++)
            {
                distance = disFunc.binaryDistance(newTimeseries.data, TimeseriesArrBin[i].data);
                if (bestsoFar > distance)
                {
                    bestsoFar = distance;
                    bestIndex = i;
                }
            }

            nhan = TimeseriesArrBin[bestIndex].Nhan;
        }

        public void classify(RealData newTimeseries, out string nhan)
        {
            // find best match
            int bestIndex = 0;
            float bestsoFar = float.MaxValue;
            float distance = 0;

            for (int i = 0; i < realData.Length; i++)
            {
                distance = disFunc.euclidSquare(newTimeseries.data, realData[i].data);
                if (bestsoFar > distance)
                {
                    bestsoFar = distance;
                    bestIndex = i;
                }
            }

            nhan = realData[bestIndex].Nhan;
        }
    }
}

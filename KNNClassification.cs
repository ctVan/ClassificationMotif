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
        int K;
        KNNClassification(BinaryData [] dataArr, int K, AbstractDistanceFunction _disFunc) {
            this.TimeseriesArrBin = dataArr;
            this.disFunc = _disFunc;
            this.K = K;
        }

        public void classify(BinaryData newTimeseries, out String nhan) {
            // find the best match with this 
            List<int> cluster = new List<int>();
            int numOfLoop = K;
            while (numOfLoop > 0) {
                // find best match and not in list "cluster"
                int bestIndex = 0;
                float bestsoFar = 0;
                float distance = 0;
                for (int i = 0; i < TimeseriesArrBin.Length; i++)
                {
                    distance = disFunc.binaryDistance(newTimeseries.data, TimeseriesArrBin[i].data);
                    if (bestsoFar > distance) {
                        if (cluster.Contains(i))
                            continue;
                        // if do not find before
                        bestsoFar = distance;
                        bestIndex = i;
                    }
                }
                numOfLoop--;
            }
            // have a cluster of new time series, do a majority vote
            // just use 1-NN 
            if (cluster.Count == 0)
                nhan = null;
            else
                nhan = newTimeseries.Nhan;
        }
    }
}

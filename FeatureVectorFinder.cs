using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    struct MotiFeature{
        public int location;
        public int length;
        MotiFeature(int loc, int len) {
            this.location = loc;
            this.length = len;
        }
    }
    class FeatureVectorFinder
    {
        
        FeatureVectorFinder() { }
        public BinaryData[] findFeatureVector() {
            IDataLoader dataLoader = new DataLoader();
            RealData[] realData = dataLoader.readReadData();
            int N = realData.Length;
            MotiFeature[] motifArr = new MotiFeature[N];

            AbstractMotifFinder motifFinder = new ExPointMotifFinder(null, 0, 0, new EuclideanDistance(null, 0));          
            int motifLoc;
            int[] motifMatches;
            long[] ExtremePointArr;
            int isRatio;
            // location of motif and its length
            int begin, lenMotif;
            // find motif each time series (definately each one have 1 match of motif)
            for (int i = 0; i < N; i++)
            {
                motifFinder.setData(realData[i].data);
                isRatio = 1;
                // estimate length of motif
                motifFinder.findMotif(out motifLoc, out motifMatches, out ExtremePointArr, isRatio);
                // find motif
                motifFinder.findMotif(out motifLoc, out motifMatches, out ExtremePointArr, 0);

                // draw a first motif
                begin = (int)ExtremePointArr[motifLoc * 2];
                lenMotif = (int)(ExtremePointArr[motifLoc * 2 + 2] - ExtremePointArr[motifLoc * 2]);
                motifArr[i].location = begin;
                motifArr[i].length = lenMotif;
            }
            // have all motif of time series
            // -> get feature vecter of them

            // result for return
            BinaryData[] TimeseriesArrBin = new BinaryData[N];
            // epsilon to determine if subsequence (motif) in current time series
            float e = 0.001f;


            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    // motif is gotten from this time series
                    if(i == j)
                    {
                        TimeseriesArrBin[i].data[j] = true;
                        continue;
                    }

                    // else 
                    TimeseriesArrBin[i].data[j] = realData[i].exist(realData[j].data, motifArr[j].location, motifArr[j].length, e);
                }
            }
            return TimeseriesArrBin;
        }
    }
}

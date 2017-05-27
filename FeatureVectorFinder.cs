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
        float epsilon;
        public FeatureVectorFinder()
        {
            this.epsilon = 0;
        }
        public BinaryData[] findFeatureVector(RealData[] realData) {
            int N = realData.Length;
            MotiFeature[] motifArr = new MotiFeature[N];

            AbstractMotifFinder EXMotifFinder = new ExPointMotifFinder(null, 0, 0, new EuclideanDistance(null, 0));
            AbstractMotifFinder MKMotifFinder = new MKMotifFinder(null, 0, 0, new EuclideanDistance(null, 0));  
            int motifLoc;
            int[] motifMatches;
            int lenMotif;

            // sum of all distance between pairs of motif
            float sumEpsilon = 0;

            int c = 0;
            // find motif each time series (definately each one have 1 match of motif)
            for (int i = 0; i < N; i++)
            {
                
                EXMotifFinder.setData(realData[i].data);
                MKMotifFinder.setData(realData[i].data);
                // estimate length of motif                
                EXMotifFinder.estimateSlidingWindow(out lenMotif);
     //           Console.WriteLine("train i= " + i.ToString()+" :" + lenMotif.ToString() + " " + c.ToString());
                if (lenMotif == 0)
                    c++;
                // find motif
                MKMotifFinder.setSlidingWindow(lenMotif);
                sumEpsilon += MKMotifFinder.findMotif(out motifLoc,out motifMatches);

                // get a first instance as motif of time series
                motifArr[i].location = motifLoc;
                motifArr[i].length = lenMotif;
            }
            // have all motif of time series
            // -> get feature vecter of them
            
            // result for return
            BinaryData[] TimeseriesArrBin = new BinaryData[N];
            // estimate epsilon: e = avg * 1.1
            epsilon = (sumEpsilon / N) * 1.1f;


            for (int i = 0; i < N; i++)
            {
                TimeseriesArrBin[i] = new BinaryData(N);
                TimeseriesArrBin[i].Nhan = realData[i].Nhan;
                for (int j = 0; j < N; j++)
                {
                    // motif is gotten from this time series
                    if(i == j)
                    {
                        TimeseriesArrBin[i].data[j] = true;
                        continue;
                    }

                    // else 
                    TimeseriesArrBin[i].data[j] = realData[i].exist(realData[j].data, motifArr[j].location, motifArr[j].length, epsilon);
                }
            }
            return TimeseriesArrBin;
        }
    }
}

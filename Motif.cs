using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

/*
ECG: R = 1.0, n = 400
ERP: R = 3.0, n = 320
Memory: R = 3.0, n = 320
Power: R = 0.01, n = 675
Stock: R = 0.01, n = 400
*/

namespace ClassificationMotif
{
    public partial class Motif : Form
    {
        char separator;             // support for both liux/window
        IDataLoader dataLoader;     // load time series data
        float[] data;
        BinaryData[] binaryData;    // result after learning (feature vector of each time serie)
        RealData[] realData;        // real data using for k-nn
        long timeToRunFV;           // time for FV classification
        public Motif()
        {
            separator = Path.DirectorySeparatorChar;
            dataLoader = new DataLoader();
            InitializeComponent();
        }

        // this function call all method to complete task
        private void runBtn_Click(object sender, EventArgs e)
        {
            // passing data to motif finder
            int slidingWindow = Int32.Parse(SdwTxt.Text);
            float R = float.Parse(RTxt.Text);

            // need to be changed in motif finder
            AbstractMotifFinder motifFinder = new ExPointMotifFinder(data, slidingWindow, R, new EuclideanDistance(data, slidingWindow));
            int motifLoc;
            int[] motifMatches;
            long[] ExtremePointArr;


            int isRatio = 1;
            // estimate length of motif
            motifFinder.findMotif(out motifLoc, out motifMatches, out ExtremePointArr, isRatio);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            System.Console.WriteLine("\nBegin finding motif ...");
            motifFinder.findMotif(out motifLoc, out motifMatches, out ExtremePointArr, 0);
            System.Console.WriteLine("Motif finding finish");
            watch.Stop();
            System.Console.WriteLine("Time to find motif : " + watch.ElapsedMilliseconds.ToString());

            // draw chart line
            for (int i = 0; i < data.Length; i++)
            {
                chartLine.Series["rawData"].Points.AddXY(i, data[i]);
            }
            for (int i = 0; i < ExtremePointArr.Length; i++)
            {
                chartLine.Series["motif1"].Points.AddXY(ExtremePointArr[i], data[ExtremePointArr[i]]);
            }
            int begin, lenMotif;

            /*if (motifLoc != -1)
            {

                // draw a first motif
                begin = (int)ExtremePointArr[motifLoc * 2];
                lenMotif = (int)(ExtremePointArr[motifLoc * 2 + 2] - ExtremePointArr[motifLoc * 2]);
                for (int i = begin; i < begin + lenMotif; i++)
                {
                    chartLine.Series["motifElement1"].Points.AddXY(i, data[i] - 3);
                }



                // draw a list of motif, with MK just 1 motif in list
                foreach (int loc in motifMatches)
                {
                    begin = (int)ExtremePointArr[loc * 2];
                    lenMotif = (int)(ExtremePointArr[loc * 2 + 2] - ExtremePointArr[loc * 2]);
                    Console.WriteLine("length motif 1: " + lenMotif.ToString());
                    for (int i = begin; i < begin + lenMotif; i++)
                    {
                        chartLine.Series["motif1"].Points.AddXY(i, data[i]);
                    }
                }


                // draw motif element
                // draw a first motif
                begin = (int)ExtremePointArr[motifLoc * 2];
                lenMotif = (int)(ExtremePointArr[motifLoc * 2 + 2] - ExtremePointArr[motifLoc * 2]);
                for (int i = begin; i < begin + lenMotif; i++)
                {
                    chartLine.Series["motif2"].Points.AddXY(i, data[i]);
                }
                Console.WriteLine("length motif 2: " + lenMotif.ToString());

                // draw a list of motif, with MK just 1 motif in list
                foreach (int loc in motifMatches)
                {
                    begin = (int)ExtremePointArr[loc * 2];
                    lenMotif = (int)(ExtremePointArr[loc * 2 + 2] - ExtremePointArr[loc * 2]);
                    for (int i = begin; i < begin + lenMotif; i++)
                    {
                        chartLine.Series["motifElement2"].Points.AddXY(i, data[i] - 3);
                    }
                }

            }*/

            chartLine.Series["rawData"].Color = Color.Blue;
            chartLine.Series["motif1"].Color = Color.Red;
            chartLine.Series["motif2"].Color = Color.Black;
            chartLine.Series["motifElement1"].Color = Color.Red;
            chartLine.Series["motifElement2"].Color = Color.Blue;

        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            SdwTxt.Text = "2";
            RTxt.Text = "1";
            OpenFileDialog fileChooser = new OpenFileDialog();
            string currentDir = Directory.GetCurrentDirectory();
            string path = Directory.GetParent(currentDir).Parent.FullName;
            fileChooser.InitialDirectory = path + separator + "data";
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // load the data
                    data = dataLoader.readFile(fileChooser.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        } 

        private void btnMKmotif_Click(object sender, EventArgs e)
        {

            // passing data to motif finder
            int slidingWindow = Int32.Parse(SdwTxt.Text);
            float R = float.Parse(RTxt.Text);

            // need to be changed in motif finder
            AbstractMotifFinder motifFinder = new MKMotifFinder(data, slidingWindow, R, new EuclideanDistance(data, slidingWindow));
            int motifLoc;
            int[] motifMatches;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            System.Console.WriteLine("\nBegin finding motif ...");
            motifFinder.findMotif(out motifLoc, out motifMatches);
            System.Console.WriteLine("Motif finding finish");
            watch.Stop();
            System.Console.WriteLine("Time to find motif : " + watch.ElapsedMilliseconds.ToString());

            // draw chart line
            for (int i = 0; i < data.Length; i++)
            {
                chartLine.Series["rawData"].Points.AddXY(i, data[i]);
            }
            if (motifLoc != -1)
            {
                /*
                // draw a first motif
                for (int i = motifLoc; i < motifLoc + slidingWindow; i++)
                {
                    chartLine.Series["motifElement1"].Points.AddXY(i, data[i] - 3);
                }
                */


                // draw a list of motif, with MK just 1 motif in list
                foreach (int loc in motifMatches)
                {
                    for (int i = loc; i < loc + slidingWindow; i++)
                    {
                        chartLine.Series["motif1"].Points.AddXY(i, data[i]);
                    }
                }

                for (int i = motifLoc; i < motifLoc + slidingWindow; i++)
                {
                    chartLine.Series["motif2"].Points.AddXY(i, data[i]);
                }

                /*
                // draw a list of motif, with MK just 1 motif in list
                foreach (int loc in motifMatches)
                {
                    for (int i = loc; i < loc + slidingWindow; i++)
                    {
                        chartLine.Series["motifElement2"].Points.AddXY(i, data[i] - 3);
                    }
                }
                */
            }
            chartLine.Series["rawData"].Color = Color.Blue;
            chartLine.Series["motif1"].Color = Color.Red;
            chartLine.Series["motif2"].Color = Color.Black;
            chartLine.Series["motifElement1"].Color = Color.Red;
            chartLine.Series["motifElement2"].Color = Color.Blue;
        }

        private void btnTrainFV_Click(object sender, EventArgs e)
        {
            IDataLoader dataLoader = new DataLoader();
            RealData[] realData;
            OpenFileDialog fileChooser = new OpenFileDialog();
            string currentDir = Directory.GetCurrentDirectory();
            string path = Directory.GetParent(currentDir).Parent.FullName;
            fileChooser.InitialDirectory = path + separator + "dataset";
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // load the data
                    realData = dataLoader.readRealData(fileChooser.FileName);

                    // draw chart line
                    for (int i = 0; i < realData[0].data.Length; i++)
                    {
                        chartLine.Series["rawData"].Points.AddXY(i, realData[4].data[i]);
                    }

                    FeatureVectorFinder fvf = new FeatureVectorFinder();

                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    System.Console.WriteLine("\nBegin FV learning ...");
                    timeToRunFV = 0;

                    binaryData = fvf.findFeatureVector(realData);

                    System.Console.WriteLine("FV learning finding finish");
                    watch.Stop();
                    timeToRunFV += watch.ElapsedMilliseconds;
                    System.Console.WriteLine("Time to learning motif : " + timeToRunFV.ToString());

                    Console.WriteLine("TRAIN finish: " + fileChooser.SafeFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }
        private void btnTestFV_Click(object sender, EventArgs e)
        {
            IDataLoader dataLoader = new DataLoader();
            RealData[] testData;
            OpenFileDialog fileChooser = new OpenFileDialog();
            string currentDir = Directory.GetCurrentDirectory();
            string path = Directory.GetParent(currentDir).Parent.FullName;
            fileChooser.InitialDirectory = path + separator + "dataset";
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // load the data
                    testData = dataLoader.readRealData(fileChooser.FileName);
                    FeatureVectorFinder fvf = new FeatureVectorFinder();

                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    System.Console.WriteLine("\nBegin FV clasification ...");

                    binaryData = fvf.findFeatureVector(testData);

                    KNNClassification knn = new KNNClassification(binaryData, new EuclideanDistance(null, 0));
                    string nhan;
                    int count = 0;
                    for (int i = 0; i < testData.Length; i++)
                    {
    //                    Console.WriteLine("test i: " + i.ToString());
                        knn.classify(binaryData[i], out nhan);
                        if (nhan.Equals(binaryData[i].Nhan))
                            count++;
                    }
                    System.Console.WriteLine("Clasification FV finding finish");
                    watch.Stop();
                    timeToRunFV += watch.ElapsedMilliseconds;
                    System.Console.WriteLine("Total time for classification " + fileChooser.SafeFileName + ": " + timeToRunFV.ToString());

                    Console.WriteLine("FV percent: " + (count * 1.0f / testData.Length).ToString());
                    Console.WriteLine("FV percent: " + count.ToString() + "/" + testData.Length.ToString());


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }


        private void btn_testKNN_Click(object sender, EventArgs e)
        {
            IDataLoader dataLoader = new DataLoader();
            RealData[] testData;

            OpenFileDialog fileChooser = new OpenFileDialog();
            string currentDir = Directory.GetCurrentDirectory();
            string path = Directory.GetParent(currentDir).Parent.FullName;
            fileChooser.InitialDirectory = path + separator + "dataset";
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // load the data
                    testData = dataLoader.readRealData(fileChooser.FileName);

                    KNNClassification knn = new KNNClassification(realData, new EuclideanDistance(null, 0));
                    string nhan;
                    int count = 0;

                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    System.Console.WriteLine("\nBegin KNN clasify ...");

                    for (int i = 0; i < testData.Length; i++)
                    {
    //                    Console.WriteLine("test i: " + i.ToString());
                        knn.classify(testData[i], out nhan);
                        if (nhan.Equals(testData[i].Nhan))
                            count++;
                    }

                    System.Console.WriteLine("Clasification KNN finding finish");
                    watch.Stop();
                    System.Console.WriteLine("Time to KNN classification  : "+ fileChooser.SafeFileName+":  " + watch.ElapsedMilliseconds.ToString());
                    Console.WriteLine("FV percent: " + (count * 1.0f / testData.Length).ToString());
                    Console.WriteLine("FV percent: " + count.ToString() + "/" + testData.Length.ToString());

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btn_bronwnKNN_Click(object sender, EventArgs e)
        {
            IDataLoader dataLoader = new DataLoader();
            OpenFileDialog fileChooser = new OpenFileDialog();
            string currentDir = Directory.GetCurrentDirectory();
            string path = Directory.GetParent(currentDir).Parent.FullName;
            fileChooser.InitialDirectory = path + separator + "dataset";
            if (fileChooser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // load the data
                    realData = dataLoader.readRealData(fileChooser.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}

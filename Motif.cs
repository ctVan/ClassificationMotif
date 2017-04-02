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
            //AbstractMotifFinder motifFinder = new MotifFinder(data, slidingWindow, R, new EucleanDistanceArray(data, slidingWindow));
            AbstractMotifFinder motifFinder = new ExPointMotifFinder(data, slidingWindow, R, new EuclideanDistance(data, slidingWindow));
            int motifLoc;
            int[] motifMatches;
            long[] ExtremePointArr;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            System.Console.WriteLine("\nBegin finding motif ...");
            motifFinder.findMotif(out motifLoc, out motifMatches, out ExtremePointArr);
            System.Console.WriteLine("Motif finding finish");
            watch.Stop();
            System.Console.WriteLine("Time to find motif : " + watch.ElapsedMilliseconds.ToString());

            // draw chart line
            for (int i = 0; i < data.Length; i++)
            {
                chartLine.Series["rawData"].Points.AddXY(i, data[i]);
            }

            
            int begin, lenMotif;
            if (motifLoc != -1)
            {
                /*
                // draw a first motif
                begin = (int)ExtremePointArr[motifLoc * 2];
                lenMotif = (int)(ExtremePointArr[motifLoc * 2 + 2] - ExtremePointArr[motifLoc * 2]);
                for (int i = begin; i < begin + lenMotif; i++)
                {
                    chartLine.Series["motif"].Points.AddXY(i, data[i]);
                }

                */
                // draw a list of motif, with MK just 1 motif in list
                foreach (int loc in motifMatches)
                {
                    begin = (int)ExtremePointArr[loc * 2];
                    lenMotif = (int)(ExtremePointArr[loc * 2 + 2] - ExtremePointArr[loc * 2]);
                    for (int i = begin; i < begin + lenMotif; i++)
                    {
                        chartLine.Series["motif"].Points.AddXY(i, data[i]);
                    }
                }


                // draw motif element
                // draw a first motif
                begin = (int)ExtremePointArr[motifLoc * 2];
                lenMotif = (int)(ExtremePointArr[motifLoc * 2 + 2] - ExtremePointArr[motifLoc * 2]);
                for (int i = begin; i < begin + lenMotif; i++)
                {
                    chartLine.Series["motifElement"].Points.AddXY(i, data[i]);
                }
                /*
                // draw a list of motif, with MK just 1 motif in list
                foreach (int loc in motifMatches)
                {
                    begin = (int)ExtremePointArr[loc * 2];
                    lenMotif = (int)(ExtremePointArr[loc * 2 + 2] - ExtremePointArr[loc * 2]);
                    for (int i = begin; i < begin + lenMotif; i++)
                    {
                        chartLine.Series["motifElement"].Points.AddXY(i, data[i] - 2);
                    }
                }*/
            }

            chartLine.Series["rawData"].Color = Color.Blue;
            chartLine.Series["motif"].Color = Color.Red;
            chartLine.Series["motifElement"].Color = Color.Red;

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
    }
}

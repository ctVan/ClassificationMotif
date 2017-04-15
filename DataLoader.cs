using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationMotif
{
    public struct RealData
    {
        public float[] data;
        public String Nhan;
        public bool exist(float[] timeserie, int motifInx, int lengthMotif, float r) {
            return false;
        }
    }
    public struct BinaryData
    {
        public bool[] data;
        public String Nhan;
    }
    public interface IDataLoader
    {
        float[] readFile(string fileName);
        RealData[] readRealData(string path);
    }

    public class DataLoader : IDataLoader
    {
        public RealData[] readRealData(string path)
        {
            List<RealData> realData = new List<RealData>();
            string line;
            

            System.IO.StreamReader file = null;
            System.Console.WriteLine("Opening file " + path + " ...");
            try
            {// try open file
                file = new System.IO.StreamReader(path);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error : " + e.Message.ToString());
                System.Environment.Exit(1);
            }

            char[] delimiterChars = { ',' };
            string[] words;

            // read all data
            System.Console.WriteLine("Reading data ...");
            while ((line = file.ReadLine()) != null)
            {
                RealData obj = new RealData();
                List<float> dataList = new List<float>();
                words = line.Split(delimiterChars);
                obj.Nhan = words[0].ToString();
                for (int i = 1; i < words.Length; i++)
                {
                    dataList.Add(float.Parse(words[i]));
                }
                obj.data = dataList.ToArray();
                realData.Add(obj);
            }
            

            // close file
            System.Console.WriteLine("Finish reading data");
      //      System.Console.WriteLine(dataList.Count.ToString() + " data imported");
            file.Close();
            file.Dispose();

            // convert back to array and return the array
            return realData.ToArray();
        }

        public float[] readFile(string fileName)
        {
            string line;
            List<float> dataList = new List<float>();

            System.IO.StreamReader file = null;
            System.Console.WriteLine("Opening file " + fileName + " ...");
            try
            {// try open file
                file = new System.IO.StreamReader(fileName);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error : " + e.Message.ToString());
                System.Environment.Exit(1);
            }

            // read all data
            System.Console.WriteLine("Reading data ...");
            while ((line = file.ReadLine()) != null)
                dataList.Add(float.Parse(line));

            // close file
            System.Console.WriteLine("Finish reading data");
            System.Console.WriteLine(dataList.Count.ToString() + " data imported");
            file.Close();
            file.Dispose();

            // convert back to array and return the array
            return dataList.ToArray();
        }
    }
}

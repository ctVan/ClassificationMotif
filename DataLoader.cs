using System;
using System.Collections.Generic;

namespace FindingMotifDiscord
{
	public interface IDataLoader
	{
		float[] readFile(string fileName);
	}

	public class DataLoader : IDataLoader
	{
		public float[] readFile(string fileName)
		{
			string line;
			List<float> dataList = new List<float>();

			System.IO.StreamReader file = null;
			System.Console.WriteLine ("Opening file " + fileName + " ...");
			try {// try open file
				file = new System.IO.StreamReader (fileName);
			} catch (Exception e) {
				System.Console.WriteLine ("Error : " + e.Message.ToString());
				System.Environment.Exit (1);
			}

			// ignore the first line
			//System.Console.WriteLine("Ignore the first line ...");
			//file.ReadLine();

			// read all data
			System.Console.WriteLine("Reading data ...");
			while ((line = file.ReadLine ()) != null)
				dataList.Add (float.Parse (line));

			// close file
			System.Console.WriteLine("Finish reading data");
			System.Console.WriteLine (dataList.Count.ToString() + " data imported");
			file.Close();
			file.Dispose();

			// convert back to array and return the array
			return dataList.ToArray();
		}
	}
}


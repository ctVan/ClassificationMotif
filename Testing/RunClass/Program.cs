using System;
using FindingMotifDiscord;
using System.IO;

namespace RunClass
{
	class MainClass
	{
        static char s = Path.DirectorySeparatorChar;
        static string motifFolder = ".." + s + ".." + s +".." + s + ".." + s + "data" + s + "motif"+ s;
        public static void Main (string[] args)
		{
			// Loading data
			IDataLoader dataLoader = new DataLoader ();
            
            float[] data = dataLoader.readFile (motifFolder + "memory.dat");

			// Finding the sliding window
			FindingSlidingWindow findSlidingWindow = new AverageSlidingWindow ();
			int slidingWindow = findSlidingWindow.findingSlidingWindow (data);

			// display the result
			System.Console.WriteLine ("Sliding window found is: " + slidingWindow);
			System.Console.ReadKey();
		}
	}
}

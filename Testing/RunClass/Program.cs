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
			//runSomething();
			runHomothety();

			System.Console.ReadKey();
		}

		public static void runHomothety()
		{
			float[] data = { 
				1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
			};
			int length = 10;

			Homothety homothey = new Homothety (length);
			float[] transform = homothey.transform (data);

			foreach (float val in transform)
				System.Console.Write (val + " ");
		}

		public static void runSomething()
		{
			// Loading data
			IDataLoader dataLoader = new DataLoader ();

			float[] data = dataLoader.readFile (motifFolder + "memory.dat");

			// Finding the sliding window
			FindingSlidingWindow findSlidingWindow = new AverageSlidingWindow ();
			int slidingWindow = findSlidingWindow.findingSlidingWindow (data);

			// display the result
			System.Console.WriteLine ("Sliding window found is: " + slidingWindow);
		}
	}
}

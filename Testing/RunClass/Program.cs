using System;
using FindingMotifDiscord;

namespace RunClass
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Loading data
			IDataLoader dataLoader = new DataLoader ();
			float[] data = dataLoader.readFile ("/home/beekill/playground/csharp/FindingMotifDiscord/data/motif/memory.dat");

			// Finding the sliding window
			FindingSlidingWindow findSlidingWindow = new AverageSlidingWindow ();
			int slidingWindow = findSlidingWindow.findingSlidingWindow (data);

			// display the result
			System.Console.WriteLine ("Sliding window found is: " + slidingWindow);
			System.Console.ReadKey();
		}
	}
}

using System;

namespace FindingMotifDiscord
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// reading data
			IDataLoader dataLoader = new DataLoader ();
			string fileName;
			System.Console.Write ("Input data file : ");
			fileName = System.Console.ReadLine ();
			float[] data = dataLoader.readFile (fileName);

			// passing data to motif finder
			const int slidingWindow = 10;
			const float R = 0.01f;
			AbstractMotifFinder motifFinder = new MotifFinder(data, 5, R);
			int motifLoc;
			int[] motifMatches;

			var watch = System.Diagnostics.Stopwatch.StartNew ();
			System.Console.WriteLine ("\nBegin finding motif ...");
			motifFinder.findMotif (out motifLoc, out motifMatches);
			System.Console.WriteLine ("Motif finding finish");
			watch.Stop ();

			System.Console.WriteLine ("\nFound motif at location: " + motifLoc.ToString ());
			System.Console.WriteLine ("Motif matches : ");
			foreach (int motif in motifMatches) {
				System.Console.WriteLine (motif.ToString ());
			}

			System.Console.WriteLine ("Time to find motif : " + watch.ElapsedMilliseconds.ToString ());
		}
	}
}

using System;

namespace FindingMotifDiscord
{
	public class Running
	{
		private bool isTimeProfiling;

		public Running ()
		{
			isTimeProfiling = false;
		}

		public void setTimeProfiling(bool value)
		{
			isTimeProfiling = value;
		}

		public void runMotifFinder(AbstractMotifFinder finder)
		{
			int motifLoc;
			int[] motifMatches;

			System.Diagnostics.Stopwatch watch = null;

			// if we set time profiling, initialize stop watch to count time
			if (isTimeProfiling)
				watch = System.Diagnostics.Stopwatch.StartNew ();

			// finding motif
			System.Console.WriteLine ("Start finding motif ...");
			finder.findMotif (out motifLoc, out motifMatches);

			// if we set time profiling, stop the count time watch
			if (isTimeProfiling)
				watch.Stop ();


			// display the result to the user
			System.Console.WriteLine("\nFound motif at location: " + motifLoc.ToString());
			System.Console.WriteLine("Motif matches : ");
			foreach (int motif in motifMatches)
			{
				System.Console.WriteLine(motif.ToString());
			}

			// output the result to file
			OutputResult outputFile = new PlainTextFileOutput ();
			outputFile.outputResult ("result.txt", "motif.txt", motifLoc, motifMatches);

			// display the time profiling information, if neccessary
			if (isTimeProfiling)
				System.Console.WriteLine("Time to find motif : " + watch.ElapsedMilliseconds.ToString());
		}

		public void runDiscordFinder(AbstractDiscordFinder finder)
		{
			int discordLoc;
			float largestDistance;

			System.Diagnostics.Stopwatch watch = null;

			// if we set time profiling, initialize stop watch to count time
			if (isTimeProfiling)
				watch = System.Diagnostics.Stopwatch.StartNew ();

			// finding motif
			System.Console.WriteLine ("Start finding motif ...");
			finder.findDiscord (out discordLoc, out largestDistance);

			// if we set time profiling, stop the count time watch
			if (isTimeProfiling)
				watch.Stop ();


			// display the result to the user
			System.Console.WriteLine("\nFound discord at location: " + discordLoc.ToString());
			System.Console.WriteLine("Largest distance: " + largestDistance.ToString());

			if (isTimeProfiling)
				System.Console.WriteLine("Time to find discord : " + watch.ElapsedMilliseconds.ToString());
		}
	}
}


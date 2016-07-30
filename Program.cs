using System;
using System.IO;

namespace FindingMotifDiscord
{
	class MainClass
	{
        // this file path uses in debug mode
        static char s = Path.DirectorySeparatorChar;
        static string discordFolder = ".." + s + ".." + s + "data" + s + "discord";
        static string motifFolder = ".." + s + ".." + s + "data" + s + "motif";

		// class to run motif/discord finder algorithm
		static private Running running;

        static string getDiscordFileName(string folderName)
        {
            string[] fileNames = Directory.GetFiles(folderName);
            int i = 1;
            
            foreach (string name in fileNames)
            {
                string n = name.Replace(discordFolder + "\\", "");
                System.Console.Write(i.ToString()+". ");
                System.Console.WriteLine(n);
                i++;
            }
            System.Console.WriteLine("\nSelect file to compute:");
            string idx = System.Console.ReadLine();

            return fileNames[Int32.Parse(idx) - 1];
        }

        static string getMotifFileName(string folderName)
        {
            string[] fileNames = Directory.GetFiles(folderName);
            int i = 1;
            foreach (string name in fileNames)
            {
                string n = name.Replace(motifFolder + "\\", "");
                System.Console.Write(i.ToString()+". ");
                System.Console.WriteLine(n);
                i++;
            }
            System.Console.WriteLine("\nSelect file to compute:");
            string idx = System.Console.ReadLine();


            return fileNames[Int32.Parse(idx) - 1];
        }

        public static void motifFindind(float[] data)
        {
            // passing data to motif finder
            const int slidingWindow = 128;
            const float R = 105f;
            // need to be changed in motif finder
            AbstractMotifFinder motifFinder = new MotifFinder(data, slidingWindow, R, new EucleanDistanceArray(data,slidingWindow));
            int motifLoc;
            int[] motifMatches;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            System.Console.WriteLine("\nBegin finding motif ...");
            motifFinder.findMotif(out motifLoc, out motifMatches);
            System.Console.WriteLine("Motif finding finish");
            watch.Stop();

            System.Console.WriteLine("\nFound motif at location: " + motifLoc.ToString());
            System.Console.WriteLine("Motif matches : ");
            foreach (int motif in motifMatches)
            {
                System.Console.WriteLine(motif.ToString());
            }
            
            System.Console.WriteLine("Time to find motif : " + watch.ElapsedMilliseconds.ToString());
        }
        
        public static void discordFinding_dp(float [] data)
        {
            // passing data to motif finder
            const int slidingWindow = 100;
            AbstractDiscordFinder discordFinder = new DiscordFinder(data, slidingWindow, new ImprovedEucleanDistance(data, slidingWindow));
            int discordLoc;
            float largestDis;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            System.Console.WriteLine("\nBegin finding discord by dynamic programming ...");
            discordFinder.findDiscord(out discordLoc, out largestDis);
            System.Console.WriteLine("Discord finding finish");
            watch.Stop();

            System.Console.WriteLine("\nFound discord at location: " + discordLoc.ToString());
            System.Console.WriteLine("Largest distance: " + largestDis.ToString());

            System.Console.WriteLine("Time to find discord : " + watch.ElapsedMilliseconds.ToString());
        }

        public static void discordFinding_bf(float[] data)
        {
            // passing data to motif finder
            const int slidingWindow = 128;

            AbstractDiscordFinder discordFinder = new DiscordFinder(data, slidingWindow, new EucleanDistance(data, slidingWindow));      // true means using brute force algorithm
            int discordLoc;
            float largestDis;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            System.Console.WriteLine("\nBegin finding discord by brute force ...");
            discordFinder.findDiscord(out discordLoc, out largestDis);
            System.Console.WriteLine("Discord finding finish");
            watch.Stop();

            System.Console.WriteLine("\nFound discord at location: " + discordLoc.ToString());
            System.Console.WriteLine("Largest distance: " + largestDis.ToString());

            System.Console.WriteLine("Time to find discord : " + watch.ElapsedMilliseconds.ToString());
        }

        public static void Main (string[] args)
		{
			// init running class
			running = new Running ();
			running.setTimeProfiling (true);

			// prompt user to choose between algorithms
            string ch;
            System.Console.Write("Motif or Discord (M/D): ");
            ch = System.Console.ReadLine();

            // get data file name
            string fileName;
            if (ch.Equals("m") || ch.Equals("M"))
                fileName = getMotifFileName(motifFolder);
            else                           
                fileName = getDiscordFileName(discordFolder);

			// load the data
			IDataLoader dataLoader = new DataLoader();
            float[] data = dataLoader.readFile (fileName);

			// Testing MK algorithm
			const int slidingWindow = 128;
			const float R = 0.01f;
			AbstractMotifFinder mkAlgorithm = new MKAlgorithm(data, slidingWindow, R /* not use */);
			running.runMotifFinder (mkAlgorithm);

            System.Console.ReadKey();
		}
	}
}
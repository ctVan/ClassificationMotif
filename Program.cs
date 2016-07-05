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

        static string getMotifFileName()
        {
            string[] fileNames = Directory.GetFiles(motifFolder);
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

		// just for testing purpose
		// testing MKAlgorithm
		public static void testMKAlgorithm(float[] data)
		{
			// passing data to motif finder
			const int slidingWindow = 128;
			const float R = 0.01f;
			// need to be changed in motif finder
			AbstractMotifFinder motifFinder = new MKAlgorithm(data, slidingWindow, R /* not use */);
			int motifLoc;
			int[] motifMatches;

			var watch = System.Diagnostics.Stopwatch.StartNew();
			System.Console.WriteLine("\nBegin finding motif MK Algorithm ...");
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

        public static void Main (string[] args)
		{
            string ch;
            System.Console.WriteLine("Motif or Discord (M/D): ");
            ch = System.Console.ReadLine();

            // reading data
            IDataLoader dataLoader = new DataLoader();
            string fileName;
            if (ch.Equals('m') || ch.Equals('M')) {
                fileName = getDiscordFileName(motifFolder);
            }
            else {                              
                fileName = getDiscordFileName(discordFolder);
            }
			
            float[] data = dataLoader.readFile (fileName);

			//AbstractDistanceFunction func = new EucleanDistance (data, 128);
			//Console.WriteLine (func.distance (19196, 19868));
			//return;

            //discordFinding_dp(data);
            //discordFinding_bf(data);
			motifFindind (data);
			testMKAlgorithm (data);

            System.Console.ReadKey();
		}
	}
}
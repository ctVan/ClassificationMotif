using System;
using System.IO;

namespace FindingMotifDiscord
{
	class MainClass
	{
        // this file path uses in debug mode
        static string discordFolder = "..\\..\\data\\discord";
        static string motifFolder = "..\\..\\data\\motif";
        static string getDiscordFileName()
        {
            string[] fileNames = Directory.GetFiles(discordFolder);
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
            const int slidingWindow = 5;
            const float R = 0.01f;
            AbstractMotifFinder motifFinder = new MotifFinder(data, slidingWindow, R);
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
            const int slidingWindow = 5;
            AbstractDiscordFinder discordFinder = new DiscordFinder(data, slidingWindow,false);
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
            const int slidingWindow = 5;
            AbstractDiscordFinder discordFinder = new DiscordFinder(data, slidingWindow,true);      // true means using brute force algorithm
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
            
            // reading data
            IDataLoader dataLoader = new DataLoader ();
			string fileName;
            fileName = getDiscordFileName();

            float[] data = dataLoader.readFile (fileName);

 //           motifFinding(data);

            discordFinding_dp(data);
            discordFinding_bf(data);
            System.Console.ReadKey();
		}
	}
}
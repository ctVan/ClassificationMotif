using System;
using System.IO;

namespace FindingMotifDiscord
{
	public interface OutputResult
	{
		void outputResult(String fileName, String dataFilePath, int motif, int[] motifMatches);
	}

	public class PlainTextFileOutput : OutputResult
	{
		override void outputResult(String fileName, String dataFilePath, int motif, int[] motifMatches)
		{
			StreamWriter stream = new StreamWriter (fileName);
			stream.WriteLine (dataFilePath);
			stream.WriteLine (motif.ToString ());
			stream.WriteLine (motifMatches.ToString ());

			stream.Close ();
			stream.Dispose ();
		}
	}
}


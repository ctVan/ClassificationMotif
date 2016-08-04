using System;

namespace FindingMotifDiscord
{
	public abstract class FindingSlidingWindow
	{
		public FindingSlidingWindow ()
		{
		}

		public abstract int findingSlidingWindow(float[] data);
	}

	public class AverageSlidingWindow : FindingSlidingWindow
	{
		public AverageSlidingWindow()
		{

		}

		public override int findingSlidingWindow(float[] data)
		{
			return 0;
		}
	}
}


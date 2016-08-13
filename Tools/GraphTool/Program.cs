using System;
using Gtk;

namespace GraphTool
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			GraphWindow win = new GraphWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}

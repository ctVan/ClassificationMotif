using System;
using Gtk;

namespace GraphTool
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			// init graph window
			// and show it
			Gtk.Application.Init ();
			new GraphWindow ();
			Gtk.Application.Run ();
		}
	}
}

using System;

namespace GraphTool
{
	public partial class GraphWindow : Gtk.Window
	{
		public GraphWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}


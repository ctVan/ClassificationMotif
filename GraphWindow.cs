using System;
using Gtk;

namespace FindingMotifDiscord
{
	public class GraphWindow : Gtk.Window
	{
		Gtk.Button singleGraphButton;
		Gtk.Button comparisonGraphButton;

		Gtk.Window singleGraphWindow;
		Gtk.Window comparisonGraphWindow;

		float[] data;
		int slidingWindow;
		int[] locations;

		public GraphWindow(float[] data, int slidingWindow, int[] locations)
			: base ("Graph Window")
		{
			this.data = data;
			this.slidingWindow = slidingWindow;
			this.locations = locations;

			singleGraphWindow = null;
			comparisonGraphWindow = null;
			DeleteEvent += new DeleteEventHandler (OnDeleteEvent);

			SetUpUi ();

			ShowAll ();
		}

		private void SetUpUi()
		{
			SetSizeRequest (192, 100);

			singleGraphButton = new Gtk.Button ("Single-Graph");
			singleGraphButton.Clicked += new EventHandler (singleGraphButton_Click);

			comparisonGraphButton = new Gtk.Button ("Comparison Graph");
			comparisonGraphButton.Clicked += new EventHandler (comparisonGraphButton_Click);

			Gtk.VBox mainLayout = new Gtk.VBox (true, 1);
			mainLayout.PackStart (singleGraphButton, true, true, 0);
			mainLayout.PackStart (comparisonGraphButton, true, true, 0);

			Add (mainLayout);
		}

		private void singleGraphButton_Click (object o, EventArgs e)
		{
			if (singleGraphWindow == null)
				singleGraphWindow = new SingleGraphWindow (data, slidingWindow, locations);
			singleGraphWindow.ShowAll ();
		}

		private void comparisonGraphButton_Click (object o, EventArgs e)
		{
			if (comparisonGraphWindow == null)
				comparisonGraphWindow = new ComparisonGraphWindow (data, slidingWindow, locations);
			comparisonGraphWindow.ShowAll ();
		}

		private void OnDeleteEvent (object o, DeleteEventArgs e)
		{
			Application.Quit ();
			e.RetVal = true;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Gtk;
using Florence;
using Florence.GtkSharp;

namespace FindingMotifDiscord
{
	public class SingleGraphWindow : Gtk.Window
	{
		Gtk.Label graphNameLabel;
		Gtk.Button prevButton;
		Gtk.Button nextButton;
		PlotWidget plotWidget;
		InteractivePlotSurface2D[] plotSurfaces;

		int graphNumber;
		float[] data;
		int[] locations;
		int slidingWindow;

		public SingleGraphWindow (float[] data, int slidingWindow, int[] locations)
			: base ("Single Graph Window")
		{
			graphNumber = 0;
			this.data = data;
			this.slidingWindow = slidingWindow;
			this.locations = locations;

			SetUpUi ();
			displayGraph ();

			ShowAll ();
		}

		private void SetUpUi()
		{
			prevButton = new Gtk.Button ();
			prevButton.Name = "prevButton";
			prevButton.Label = "Prev";
			prevButton.Clicked += new EventHandler(prevButton_Click);

			nextButton = new Gtk.Button ();
			nextButton.Name = "nextButton";
			nextButton.Label = "Next";
			nextButton.Clicked += new EventHandler(nextButton_Click);

			graphNameLabel = new Gtk.Label ();
			graphNameLabel.Name = "graphNameLabel";

			plotWidget = new PlotWidget ();
			plotSurfaces = new InteractivePlotSurface2D[locations.Length + 1];
			for (int i = 0; i < plotSurfaces.Length; ++i)
				plotSurfaces [i] = null;

			// layout these widgets
			Gtk.HBox topLayout = new Gtk.HBox(false, 1);
			topLayout.PackStart (graphNameLabel, true, true, 0);
			topLayout.PackStart (prevButton, false, false, 0);
			topLayout.PackStart (nextButton, false, false, 0);

			Gtk.VBox mainLayout = new Gtk.VBox (false, 0);
			mainLayout.PackStart (topLayout, false, false, 0);
			mainLayout.PackStart (plotWidget);

			SetSizeRequest (632, 520);
			Add (mainLayout);
		}

		private void prevButton_Click(object sender, EventArgs e)
		{
			--graphNumber;
			if (graphNumber < 0)
				graphNumber = locations.Length;
			displayGraph ();
		}

		private void nextButton_Click(object sender, EventArgs e)
		{
			graphNumber = (graphNumber + 1) % (locations.Length + 1);
			displayGraph ();
		}

		private void displayGraph()
		{
			// create graph if necessary
			if (plotSurfaces [graphNumber] == null) {
				plotSurfaces [graphNumber] = new InteractivePlotSurface2D ();
				if (graphNumber == 0) {
					LinePlot linePlot = new LinePlot ();
					linePlot.DataSource = data;

					plotSurfaces [0].AddInteraction (new VerticalGuideline (Color.Gray));
					plotSurfaces [0].AddInteraction (new HorizontalGuideline (Color.Gray));
					plotSurfaces [0].AddInteraction (new PlotDrag (true, true));
					plotSurfaces [0].AddInteraction (new AxisDrag ());
					plotSurfaces [0].Add (linePlot);
				} else {
					LinePlot linePlot = new LinePlot ();
					linePlot.DataSource = data.Skip (locations [graphNumber - 1]).Take (slidingWindow).ToArray ();

					plotSurfaces [graphNumber].Add (linePlot);
				}
			}

			// show label
			if (graphNumber > 0)
				graphNameLabel.Text = locations [graphNumber - 1].ToString ();
			else
				graphNameLabel.Text = "What to put here?";

			// display graph
			plotWidget.InteractivePlotSurface2D = plotSurfaces[graphNumber];
			plotSurfaces [graphNumber].Refresh ();
		}
	}
}


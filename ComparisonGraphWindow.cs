using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Gtk;
using Florence;
using Florence.GtkSharp;

namespace FindingMotifDiscord
{
	public class ComparisonGraphWindow : Gtk.Window
	{
		float[] data;
		int[] locations;
		int slidingWindow;

		PlotWidget plotWidget;
		InteractivePlotSurface2D plotSurface;

		static Color[] colors = new Color[] { 
			Color.Black,
			Color.Yellow,
			Color.Purple,
			Color.Orange,
			Color.LightSteelBlue,
			Color.Red,
			Color.Gray,
			Color.Green,
			Color.Pink,
			Color.Blue,
			Color.Violet,
			Color.Orange,
			Color.RosyBrown,
			Color.YellowGreen,
			Color.SaddleBrown,
			Color.DarkOliveGreen,
			Color.SpringGreen
		};

		public ComparisonGraphWindow (float[] data, int slidingWindow, int[] locations)
			: base ("Comparison Graph Window")
		{
			this.data = data;
			this.slidingWindow = slidingWindow;
			this.locations = locations;

			SetUpUi ();
			displayGraph ();
			DeleteEvent += new DeleteEventHandler (OnDeleteEvent);

			ShowAll ();
		}

		private void SetUpUi()
		{
			plotWidget = new PlotWidget ();
			plotSurface = new InteractivePlotSurface2D ();
			plotWidget.InteractivePlotSurface2D = plotSurface;

			SetSizeRequest (632, 520);
			Add (plotWidget);
		}

		private void OnDeleteEvent (object o, DeleteEventArgs e)
		{
			HideAll ();
			e.RetVal = true;
		}

		private void displayGraph()
		{
			int i = 0;

			plotSurface.Clear ();
			foreach (int m in locations) {
				LinePlot linePlot = new LinePlot ();
				linePlot.DataSource = data.Skip (m).Take (slidingWindow).ToArray ();
				linePlot.Color = colors [i];
				linePlot.Label = m.ToString ();
				++i;

				plotSurface.Add (linePlot);
			}

			// legend
			Legend legend = new Legend();
			legend.AttachTo (PlotSurface2D.XAxisPosition.Top, PlotSurface2D.YAxisPosition.Right);
			legend.VerticalEdgePlacement = Legend.Placement.Outside;
			legend.HorizontalEdgePlacement = Legend.Placement.Inside;
			plotSurface.Legend = legend;

			plotSurface.Refresh ();
		}
	}
}


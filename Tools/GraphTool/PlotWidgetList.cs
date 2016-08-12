using System;
using WidgetsLibrary;
using System.Collections.Generic;

namespace GraphTool
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PlotWidgetList : Gtk.Bin
	{
		private List<int> locations;

		public delegate void OnPlotWidgetClicked(int location);

		public event OnPlotWidgetClicked onPlotWidgetClicked;

		public PlotWidgetList ()
		{
			this.Build ();
			locations = new List<int> ();
			horizontalwidgetslist1.widgetClicked += childWidgetClicked;
		}

		public void addPlotWidget(int location, Gtk.Widget widget)
		{
			locations.Add (location);
			horizontalwidgetslist1.Add (widget);
		}

		public void removePlotWidget(int location)
		{
			// get the index of location
			int index = locations.Find (location);

			// remove the widget at location
			Gtk.Widget[] widgets = horizontalwidgetslist1.Children;
			horizontalwidgetslist1.removeWidget (widgets [index]);

			// remove the location from locations
			locations.Remove (location);
		}

		protected void childWidgetClicked(Gtk.Widget widget)
		{
			// check whether we have a handler
			if (onPlotWidgetClicked == null)
				return;

			// if so
			// get the widget's location
			Gtk.Widget[] widgets = horizontalwidgetslist1.Children;
			int index;
			for (index = 0; index < widgets.Length; ++index)
				if (widget == widgets [index])
					break;

			// hand the data to the handler
			onPlotWidgetClicked (locations [index]);
		}
	}
}


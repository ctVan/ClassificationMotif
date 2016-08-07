using System;

namespace WidgetsLibrary
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class HorizontalWidgetsList : Gtk.Bin
	{
		public delegate void WidgetClicked(Gtk.Widget widget);

		public event WidgetClicked widgetClicked;

		public HorizontalWidgetsList ()
		{
			this.Build ();
		}

		protected void OnWidgetListFocusChildSet (object o, Gtk.FocusChildSetArgs args)
		{
			if (widgetClicked != null)
				widgetClicked (args.Widget);
		}

		public void removeWidget(Gtk.Widget widget)
		{
			widgetList.Remove (widget);
		}

		public void addWidget(Gtk.Widget widget)
		{
			widgetList.PackStart (widget);
		}
	}
}

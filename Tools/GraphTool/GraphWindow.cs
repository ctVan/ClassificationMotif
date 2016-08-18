using System;
using Gtk;
using Florence;
using Florence.GtkSharp;

public partial class GraphWindow: Gtk.Window
{	
	private void initializeGui()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget GraphWindow
		this.Name = "GraphWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("GraphWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child GraphWindow.Gtk.Container+ContainerChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		initializeMainPlotWidget ();
		this.mainPlotWidget.WidthRequest = 686;
		this.mainPlotWidget.HeightRequest = 462;
		this.mainPlotWidget.Name = "mainPlotWidget";
		this.vbox2.Add (this.mainPlotWidget);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.mainPlotWidget]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		w1.Padding = ((uint)(5));
		// Container child vbox2.Gtk.Box+BoxChild
		this.widgetList1 = new global::WidgetsLibrary.HorizontalWidgetsList ();
		this.widgetList1.Events = ((global::Gdk.EventMask)(256));
		this.widgetList1.Name = "widgetList1";

		addPlotWidgetsToWidgetList (10);
		widgetList1.widgetClicked += onWidgetListChildWidgetClicked;

		this.vbox2.Add (this.widgetList1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.widgetList1]));
		w2.Position = 1;
		this.hbox1.Add (this.vbox2);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.comparisonModeCheckBox = new global::Gtk.CheckButton ();
		this.comparisonModeCheckBox.CanFocus = true;
		this.comparisonModeCheckBox.Name = "comparisonModeCheckBox";
		this.comparisonModeCheckBox.Label = global::Mono.Unix.Catalog.GetString ("Comparison Mode");
		this.comparisonModeCheckBox.DrawIndicator = true;
		this.comparisonModeCheckBox.UseUnderline = true;
		this.vbox1.Add (this.comparisonModeCheckBox);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.comparisonModeCheckBox]));
		w4.Position = 0;
		w4.Expand = false;
		w4.Fill = false;
//		// Container child vbox1.Gtk.Box+BoxChild
//		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
//		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
//		this.GtkScrolledWindow.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
//		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
//		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
//		global::Gtk.Viewport w5 = new global::Gtk.Viewport ();
//		w5.ShadowType = ((global::Gtk.ShadowType)(0));
//		// Container child GtkViewport1.Gtk.Container+ContainerChild
//		this.listView = new global::MonoDevelop.Components.ListView ();
//		this.listView.HeightRequest = 447;
//		this.listView.Name = "listView";
//		this.listView.AllowMultipleSelection = true;
//		this.listView.SelectedRow = 0;
//		this.listView.SelectionDisabled = false;
//		this.listView.Page = 0;
//		w5.Add (this.listView);
//		this.GtkScrolledWindow.Add (w5);
//		this.vbox1.Add (this.GtkScrolledWindow);
//		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.GtkScrolledWindow]));
//		w6.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.infoLabel = new global::Gtk.Label ();
		this.infoLabel.Name = "infoLabel";
		this.infoLabel.LabelProp = global::Mono.Unix.Catalog.GetString ("label1");
		this.vbox1.Add (this.infoLabel);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.infoLabel]));
		w7.Position = 2;
		w7.Expand = false;
		w7.Fill = false;
		this.hbox1.Add (this.vbox1);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox1]));
		w8.Position = 1;
		w8.Expand = false;
		w8.Fill = false;
		this.Add (this.hbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 832;
		this.DefaultHeight = 607;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
	}

	private void addPlotWidgetsToWidgetList(int numOfWidgets)
	{
		for (int i = 0; i < numOfWidgets; ++i) {
			// create plot widget and its data
			PlotWidget plotWidget = new PlotWidget ();
			plotWidget.WidthRequest = 200;
			plotWidget.HeightRequest = 50;
			InteractivePlotSurface2D plotSurface = new InteractivePlotSurface2D ();

			float[] data = initData (50);
			LinePlot linePlot = new LinePlot ();
			linePlot.DataSource = data;

			plotSurface.Add (linePlot);
			plotWidget.InteractivePlotSurface2D = plotSurface;
			plotWidget.Show ();

			// add plot widget to widgetList
			widgetList1.addWidget((Gtk.Widget)plotWidget);
		}
	}

	private void onWidgetListChildWidgetClicked(Gtk.Widget childWidget)
	{
		System.Console.WriteLine (childWidget.ToString());
	}

	private void initializeMainPlotWidget()
	{
		mainPlotWidget = new PlotWidget ();
		InteractivePlotSurface2D plotSurface = new InteractivePlotSurface2D ();

		float[] data = initData (1000);
		LinePlot linePlot = new LinePlot ();
		linePlot.DataSource = data;

		plotSurface.Add (linePlot);
		mainPlotWidget.InteractivePlotSurface2D = plotSurface;
	}

	private float[] initData(int length)
	{
		float[] data = new float[length];
		Random random = new Random ();
		for (int i = 0; i < length; ++i)
			data [i] = (float)random.NextDouble ();

		return data;
	}

	public GraphWindow (): base (Gtk.WindowType.Toplevel)
	{
		//Build ();
		initializeGui ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace _2b2tpay.Data
{
	public class Chart : Form
	{
		public class Revenue
		{
			public int Year
			{
				get;
				set;
			}

			public int Month
			{
				get;
				set;
			}

			public int Day
			{
				get;
				set;
			}

			public double Value
			{
				get;
				set;
			}
		}

		private List<Revenue> list = new List<Revenue>
		{
			new Revenue
			{
				Year = 2020,
				Month = DateTime.Now.Month,
				Day = 1,
				Value = 4.0
			},
			new Revenue
			{
				Year = 2020,
				Month = DateTime.Now.Month,
				Day = 2,
				Value = 8.0
			},
			new Revenue
			{
				Year = 2020,
				Month = DateTime.Now.Month,
				Day = 3,
				Value = 7.0
			},
			new Revenue
			{
				Year = 2020,
				Month = DateTime.Now.Month,
				Day = 4,
				Value = 20.0
			}
		};

		private IContainer components = null;

		private CartesianChart cartesianChart1;

		public Chart()
			: this()
		{
			InitializeComponent();
		}

		private void Chart_Load(object sender, EventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Expected O, but got Unknown
			AxesCollection axisX = cartesianChart1.get_AxisX();
			Axis val = new Axis();
			val.set_Title("Month");
			val.set_Labels((IList<string>)new string[12]
			{
				"Jan",
				"Feb",
				"Mar",
				"Apr",
				"May",
				"Jun",
				"Jul",
				"Aug",
				"Sep",
				"Oct",
				"Nov",
				"Dec"
			});
			((NoisyCollection<Axis>)(object)axisX).Add(val);
			AxesCollection axisY = cartesianChart1.get_AxisY();
			Axis val2 = new Axis();
			val2.set_Title("Revenue");
			val2.set_LabelFormatter((Func<double, string>)((double value) => value.ToString("C")));
			((NoisyCollection<Axis>)(object)axisY).Add(val2);
			cartesianChart1.set_LegendLocation((LegendLocation)4);
			((NoisyCollection<ISeriesView>)(object)cartesianChart1.get_Series()).Clear();
			SeriesCollection val3 = new SeriesCollection();
			var enumerable = Enumerable.Distinct(Enumerable.Select((IEnumerable<Revenue>)this.list, (Revenue o) => new
			{
				o.Year
			}));
			foreach (var year in enumerable)
			{
				List<double> list = new List<double>();
				int month;
				for (month = 1; month <= 12; month++)
				{
					double item = 0.0;
					var enumerable2 = Enumerable.Select((IEnumerable<Revenue>)Enumerable.OrderBy<Revenue, int>(Enumerable.Where<Revenue>((IEnumerable<Revenue>)this.list, (Func<Revenue, bool>)((Revenue o) => o.Year.Equals(year.Year) && o.Month.Equals(month))), (Func<Revenue, int>)((Revenue o) => o.Month)), (Revenue o) => new
					{
						o.Value,
						o.Month
					});
					if (Enumerable.SingleOrDefault(enumerable2) != null)
					{
						item = Enumerable.SingleOrDefault(enumerable2).Value;
					}
					list.Add(item);
				}
				LineSeries val4 = new LineSeries();
				((Series)val4).set_Title(year.Year.ToString());
				((Series)val4).set_Values((IChartValues)(object)new ChartValues<double>((IEnumerable<double>)list));
				((NoisyCollection<ISeriesView>)(object)val3).Add((ISeriesView)val4);
			}
			cartesianChart1.set_Series(val3);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				((IDisposable)components).Dispose();
			}
			((Form)this).Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			cartesianChart1 = new CartesianChart();
			((Control)this).SuspendLayout();
			((Control)cartesianChart1).set_Location(new Point(-12, -1));
			((Control)cartesianChart1).set_Name("cartesianChart1");
			((Control)cartesianChart1).set_Size(new Size(800, 452));
			((Control)cartesianChart1).set_TabIndex(0);
			((Control)cartesianChart1).set_Text("cartesianChart1");
			((ContainerControl)this).set_AutoScaleDimensions(new SizeF(6f, 13f));
			((ContainerControl)this).set_AutoScaleMode((AutoScaleMode)1);
			((Form)this).set_ClientSize(new Size(800, 450));
			((Control)this).get_Controls().Add((Control)(object)cartesianChart1);
			((Control)this).set_Name("Chart");
			((Control)this).set_Text("Chart");
			((Form)this).add_Load((EventHandler)Chart_Load);
			((Control)this).ResumeLayout(false);
		}
	}
}

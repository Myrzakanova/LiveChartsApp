using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveChartsApp
{
	/// <summary>
	/// Логика взаимодействия для DetailsPage.xaml
	/// </summary>
	public partial class DetailsPage : Page, INotifyPropertyChanged
	{
		public DateTime SelectedDate { get; set; }
		public SeriesCollection PieSeries { get; set; }

		private string _winChangeText;
		private string _macChangeText;
		private string _linuxChangeText;

		public string WinChangeText
		{
			get => _winChangeText;
			set { _winChangeText = value; OnPropertyChanged(); }
		}

		public string MacChangeText
		{
			get => _macChangeText;
			set { _macChangeText = value; OnPropertyChanged(); }
		}

		public string LinuxChangeText
		{
			get => _linuxChangeText;
			set { _linuxChangeText = value; OnPropertyChanged(); }
		}

		public DetailsPage(DateTime date)
		{
			SelectedDate = date;
			InitializeComponent();
			DataContext = this;
			LoadData();
		}

		private void LoadData()
		{
			var entry = DataBase.GetEntryByDate(SelectedDate);
			if (entry == null) return;

			// Круговая диаграмма
			var total = entry.Windows + entry.Mac + entry.Linux;
			if (total == 0) total = 1;

			PieSeries = new SeriesCollection
		{
			new PieSeries
			{
				Title = "Windows",
				Values = new ChartValues<double> { entry.Windows * 100.0 / total },
				DataLabels = true,
				LabelPoint = p => $"{p.Y:N1}% ({entry.Windows})",
				Fill = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50))
			},
			new PieSeries
			{
				Title = "Mac",
				Values = new ChartValues<double> { entry.Mac * 100.0 / total },
				DataLabels = true,
				LabelPoint = p => $"{p.Y:N1}% ({entry.Mac})",
				Fill = new SolidColorBrush(Color.FromRgb(0x21, 0x96, 0xF3))
			},
			new PieSeries
			{
				Title = "Linux",
				Values = new ChartValues<double> { entry.Linux * 100.0 / total },
				DataLabels = true,
				LabelPoint = p => $"{p.Y:N1}% ({entry.Linux})",
				Fill = new SolidColorBrush(Color.FromRgb(0xFF, 0x98, 0x00))
			}
		};

			// Изменения за неделю
			var changes = StatsCalculator.GetWeeklyChanges(SelectedDate);

			WinChangeText = FormatChangeText("Windows", changes.winChange, entry.Windows);
			MacChangeText = FormatChangeText("MacOS", changes.macChange, entry.Mac);
			LinuxChangeText = FormatChangeText("Linux", changes.linuxChange, entry.Linux);

			OnPropertyChanged(nameof(PieSeries));
		}

		private string FormatChangeText(string osName, double change, int currentValue)
		{
			var arrow = change switch
			{
				> 0 => "↑",
				< 0 => "↓",
				_ => "→"
			};

			return $"{osName}\n{currentValue} ({arrow} {Math.Abs(change):N1}%)";
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

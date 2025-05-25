using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
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
	/// Логика взаимодействия для StatsPageMain.xaml
	/// </summary>
	public partial class StatsPageMain : Page, INotifyPropertyChanged
	{
		public SeriesCollection Series { get; set; }
		public List<string> Dates { get; set; }

		public StatsPageMain()
		{
			InitializeComponent();
			DataContext = this;
			LoadData();
		}

		private void LoadData()
		{
			var entries = DataBase.GetAllEntries();

			Dates = entries.Select(e => e.Date.ToString("yyyy-MM-dd")).ToList();

			Series = new SeriesCollection
		{
			new LineSeries
			{
				Title = "Windows",
				Values = new ChartValues<int>(entries.Select(e => e.Windows))
			},
			new LineSeries
			{
				Title = "Mac",
				Values = new ChartValues<int>(entries.Select(e => e.Mac))
			},
			new LineSeries
			{
				Title = "Linux",
				Values = new ChartValues<int>(entries.Select(e => e.Linux))
			}
		};
			SelectedData = entries.FirstOrDefault();
			OnPropertyChanged(nameof(Series));
			OnPropertyChanged(nameof(Dates));
		}

		private void AddNewData_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AddDataPage());
		}
	
		private void DateFilter_Changed(object sender, SelectionChangedEventArgs e)
		{
			if (DateFilter.SelectedDate is DateTime selectedDate)
			{
				NavigationService.Navigate(new DetailsPage(selectedDate));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		public SeriesCollection PieSeries { get; set; }
		private StatsEntry _selectedData;

		public StatsEntry SelectedData
		{
			get => _selectedData;
			set
			{
				_selectedData = value;
				UpdatePieChart();
				OnPropertyChanged(nameof(SelectedData));
			}
		}

		// Existing methods...
		private void UpdatePieChart()
		{
			if (SelectedData == null) return;

			var total = SelectedData.Windows + SelectedData.Mac + SelectedData.Linux;
			if (total == 0) total = 1; // Avoid division by zero

			PieSeries = new SeriesCollection
		{
			new PieSeries
			{
				Title = "Windows",
				Values = new ChartValues<double> { SelectedData.Windows * 100.0 / total },
				DataLabels = true,
				LabelPoint = p => $"{p.Y:N1}%"
			},
			new PieSeries
			{
				Title = "Mac",
				Values = new ChartValues<double> { SelectedData.Mac * 100.0 / total },
				DataLabels = true,
				LabelPoint = p => $"{p.Y:N1}%"
			},
			new PieSeries
			{
				Title = "Linux",
				Values = new ChartValues<double> { SelectedData.Linux * 100.0 / total },
				DataLabels = true,
				LabelPoint = p => $"{p.Y:N1}%"
			}
		};

			OnPropertyChanged(nameof(PieSeries));
		}
	}
}

using System;
using System.Collections.Generic;
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
	/// Логика взаимодействия для AddDataPage.xaml
	/// </summary>
	public partial class AddDataPage : Page
	{
		public AddDataPage()
		{
			InitializeComponent();
			DatePicker.SelectedDate = DateTime.Today;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(WindowsInput.Text, out int win) &&
				int.TryParse(MacInput.Text, out int mac) &&
				int.TryParse(LinuxInput.Text, out int linux))
			{
				DataBase.AddEntry(win, mac, linux, DatePicker.SelectedDate.Value);
				NavigationService.Navigate(new StatsPageMain());
			}
		}
	}
}

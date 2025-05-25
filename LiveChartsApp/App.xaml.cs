using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Windows;

namespace LiveChartsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		public static string WeatherApiKey = "7acd69a4a5859383ba0042a5c497d472";
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			DataBase.Initialize();
			MainWindow mainWindow = new MainWindow();
			mainWindow.MainFrame.Navigate(new StatsPageMain());
			mainWindow.Show();
		}
	}

}

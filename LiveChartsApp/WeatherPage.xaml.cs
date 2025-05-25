using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LiveChartsApp
{
	public partial class WeatherPage : Page
	{
		public WeatherPage()
		{
			InitializeComponent();
		}

		private async void GetWeather_Click(object sender, RoutedEventArgs e)
		{
			var city = CityInput.Text.Trim();
			if (string.IsNullOrEmpty(city))
			{
				ShowError("Введите название города");
				return;
			}

			WeatherPanel.Visibility = Visibility.Collapsed;
			ErrorText.Visibility = Visibility.Collapsed;

			var weather = await WeatherService.GetWeather(city);

			if (weather == null)
			{
				ShowError("Не удалось получить данные. Проверьте название города");
				return;
			}

			UpdateWeatherUI(weather);
		}

		private void UpdateWeatherUI(WeatherData weather)
		{
			CityText.Text = weather.City;
			TempText.Text = $"Температура: {weather.Temperature}°C";
			DescriptionText.Text = $"Состояние: {weather.Description}";
			HumidityText.Text = $"Влажность: {weather.Humidity}%";
			WindText.Text = $"Ветер: {weather.WindSpeed} м/с";

			try
			{
				WeatherIcon.Source = new BitmapImage(
					new Uri($"icons/{weather.IconCode}.png", UriKind.Relative));
			}
			catch
			{
				WeatherIcon.Source = null;
			}

			WeatherPanel.Visibility = Visibility.Visible;
		}

		private void ShowError(string message)
		{
		
			ErrorText.Visibility = Visibility.Visible;
		}
	}
}
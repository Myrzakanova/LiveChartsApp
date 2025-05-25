using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartsApp
{
	public class WeatherData
	{
		public string City { get; set; }
		public double Temperature { get; set; }
		public string Description { get; set; }
		public double Humidity { get; set; }
		public double WindSpeed { get; set; }
		public string IconCode { get; set; }
	}
	public static class WeatherService
	{
		public static async Task<WeatherData> GetWeather(string city)
		{
			using (var client = new HttpClient())
			{
				try
				{
					var response = await client.GetStringAsync(
						$"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={App.WeatherApiKey}");

					dynamic data = JsonConvert.DeserializeObject(response);

					return new WeatherData
					{
						City = city,
						Temperature = data.main.temp,
						Description = data.weather[0].description,
						Humidity = data.main.humidity,
						WindSpeed = data.wind.speed,
						IconCode = data.weather[0].icon
					};
				}
				catch
				{
					return null;
				}
			}
		}
	}
}

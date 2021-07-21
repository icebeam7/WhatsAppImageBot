using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using ImageBot.Models;

namespace ImageBot.Services
{
    public class WeatherService
    {
        private static string weatherURL = $"http://api.openweathermap.org/data/2.5/weather";
        private static string apiKey = "";
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> GetWeather(string city)
        {
            var weatherFullURL = $"{weatherURL}?appid={apiKey}&q={city}";
            var weatherResult = await httpClient.GetStringAsync(weatherFullURL);
            var weatherModel = JsonConvert.DeserializeObject<WeatherModel>(weatherResult);
            weatherModel.main.temp -= 273.15;

            return $"{weatherModel.weather.First().main} ({weatherModel.main.temp.ToString("N2")} °C)";
        }
    }
}

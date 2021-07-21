using System;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;

using ImageBot.Services;

namespace ImageBot
{
    public static class ReceiveMessage
    {
        [FunctionName("receive-message")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            var data = await req.Content.ReadAsStringAsync();
            var formValues = data.Split('&')
                .Select(value => value.Split('='))
                .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "),
                              pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));

            var isImage = (formValues["NumMedia"].ToString() == "1");
            var message = string.Empty;

            if (!isImage) // si es texto
            {
                var text = formValues["Body"].ToString();
                var city = await LuisService.GetCity(text, log);
                var weather = await WeatherService.GetWeather(city);
                message = $"The weather of {city} is: {weather}";
            }
            else // si es imagen
            {
                var url = formValues["MediaUrl0"].ToString();
                message = await ComputerVisionService.AnalyzeImage(url);
            }

            var twiml = TwilioService.GetTwilioMessage(message);

            return new HttpResponseMessage
            {
                Content = new StringContent(twiml,
                    Encoding.UTF8, "application/xml")
            };
        }
    }
}
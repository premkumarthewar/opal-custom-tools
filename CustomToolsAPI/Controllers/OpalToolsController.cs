using CustomToolsAPI.Models;
using CustomToolsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Optimizely.Opal.Tools;
using System.Text;
using System.Text.Json;

namespace CustomToolsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpalToolsController : Controller
    {
        private readonly IWeatherService weatherService;

        private readonly IGoogleGeminiService googleGeminiService;

        public OpalToolsController(IWeatherService weatherService, IGoogleGeminiService googleGeminiService)
        {
            this.weatherService = weatherService;
            this.googleGeminiService = googleGeminiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        /// <summary>
        /// Opal tool which will generate random numbers between the minimum and maximum values provided from console. This is treated as an action method in the API application.
        /// </summary>
        /// <param name="randomNumberGeneratorValues">Stores the minimum and maximum value range within which the number will be generated</param>
        /// <returns>Object having the random number.</returns>
        [OpalTool(name: "random_number_generator")]
        [HttpPost("GenerateRandomNumber")]
        public object GenerateRandomNumber(RandomNumberGeneratorValues randomNumberGeneratorValues)
        {
            Random random = new();

            int number = random.Next(randomNumberGeneratorValues.Minimum, randomNumberGeneratorValues.Maximum);

            return new { Result = number };
        }

        /// <summary>
        /// Opal tool that greets a user based on the language they ask. It currently has 5 languages which can be extended further. This is treated as an action method in the API application.
        /// </summary>
        /// <param name="language">Language in which user requests a greeting. This is a plain string</param>
        /// <returns>Object with the greeting.</returns>
        [OpalTool(name: "give_a_greeting")]
        [HttpGet("GiveAGreeting")]
        public object GiveAGreeting(string language)
        {
            string greeting = string.Empty;
            switch (language.ToLower())
            {
                case "tamil":
                    greeting = "Vanakkam makkale!";
                    break;
                case "malayalam":
                    greeting = "Namaskaram prekshakare!";
                    break;
                case "hindi":
                case "marathi":
                    greeting = "Namaste!";
                    break;
                case "english":
                    greeting = "Hello folks!";
                    break;

            }

            return new { Greeting = greeting };
        }

        /// <summary>
        /// Opal tool which gets the weather of a city. This is treated as an action method in the API application.
        /// </summary>
        /// <param name="city">Name of the city for which weather must be known. This is a plain string variable</param>
        /// <returns>Object having the weather details.</returns>
        [OpalTool("get_weather_details")]
        [HttpGet("GetWeather")]
        public async Task<object> GetWeather(string city)
        {
            WeatherResponse weatherResponse = await weatherService.GetWeatherAsync(city);

            if (weatherResponse == null || weatherResponse.Main == null || weatherResponse.Weather!.Count == 0 || string.IsNullOrEmpty(weatherResponse.Name))
                return NoContent();

            return Ok(new
            {
                City = weatherResponse.Name,
                Temperature = weatherResponse.Main?.Temp,
                weatherResponse.Main?.Humidity,
                weatherResponse.Weather?.FirstOrDefault()?.Description
            });
        }

        [OpalTool("custom_google_gemini")]
        [HttpPost("CustomGoogleGemini")]
        public async Task<object> GoogleGeminiInfo([FromBody] string prompt)
        {
            object request = new
            {
                contents = new[]
                {
                    new
                    {
                       parts = new []
                        {
                            new {text = prompt}
                        }
                    }
                }
            };

            string response = await googleGeminiService.GetResponseFromGoogleGeminiAsync(request);
            
            if (string.IsNullOrEmpty(response)) return NoContent();

            JsonDocument doc = JsonDocument.Parse(response);

            string text = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString()!;
            
            return Ok(text);
        }
    }
}
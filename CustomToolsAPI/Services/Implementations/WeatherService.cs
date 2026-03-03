using CustomToolsAPI.Models;
using CustomToolsAPI.Services.Interfaces;
using System.Text.Json;

namespace CustomToolsAPI.Services.Implementations
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Concrete implementation of the abstract method in IWeatherService interface. Business logic of fetching the weather from OpenWeatherMap API is implemented here.
        /// </summary>
        /// <param name="city">Name of the city for which weather must be known. This is a plain string variable</param>
        /// <returns>Response received from the API in deserialized form comprising of Temperature, Humidity, etc.</returns>
        /// <exception cref="Exception">Exception is thrown in case service does not return a successful response.</exception>
        public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            string apiKey = configuration["OpenWeather:ApiKey"]!;

            string baseUrl = configuration["OpenWeather:BaseUrl"]!;

            string url = $"{baseUrl}weather?q={city}&appId={apiKey}&units=metric";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Unable to fetch weather data");

            string json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(json))
                return new WeatherResponse();

            WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(json, jsonSerializerOptions)!;

            return weatherResponse!;
        }
    }
}

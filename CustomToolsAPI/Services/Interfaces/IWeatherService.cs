using CustomToolsAPI.Models;

namespace CustomToolsAPI.Services.Interfaces
{
    public interface IWeatherService
    {
        /// <summary>
        /// Gets the weather details for the city specified.
        /// </summary>
        /// <param name="city">Name of the city for which weather must be known. This is a plain string variable</param>
        /// <returns>Response received from the API in deserialized form comprising of Temperature, Humidity, etc.</returns>
        Task<WeatherResponse> GetWeatherAsync(string city);
    }
}

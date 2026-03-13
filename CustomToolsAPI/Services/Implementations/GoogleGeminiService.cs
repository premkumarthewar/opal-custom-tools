using CustomToolsAPI.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace CustomToolsAPI.Services.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class GoogleGeminiService : IGoogleGeminiService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public GoogleGeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> GetResponseFromGoogleGeminiAsync(object request)
        {
            string apiKey = configuration["GoogleGemini:ApiKey"]!;

            string json = JsonSerializer.Serialize(request);

            string modelUrl = configuration["GoogleGemini:ModelUrl"]!;

            HttpResponseMessage response = await httpClient.PostAsync($"{modelUrl}?key={apiKey}", new StringContent(json, Encoding.UTF8, "application/json"));

            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}

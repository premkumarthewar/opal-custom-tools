namespace CustomToolsAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WeatherResponse
    {
        public MainInfo? Main { get; set; }

        public List<WeatherInfo>? Weather { get; set; }

        public string? Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MainInfo
    {
        public double Temp { get; set; }

        public int Humidity { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WeatherInfo
    {
        public string? Description { get; set; }

        public string? Icon { get; set; }
    }
}

using WeatherApp.Model;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        //Signatures for methods
        Task<WeatherModel> GetWeatherAsync(string city);
        Task<List<ForecastItem>> GetForecastAsync(string city);
        Task<List<CityInfo>> GetCitySuggestionsAsync(string query);
    }
}

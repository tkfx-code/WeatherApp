using WeatherApp.Model;
using WeatherApp.Repositories;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        //Signatures for methods
        Task<WeatherModel> GetWeatherAsync(string city);
        Task<List<ForecastItem>> GetForecastAsync(string city);
    }
}

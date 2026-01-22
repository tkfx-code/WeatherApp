using System.Diagnostics;
using System.Text.Json;
using WeatherApp.Model;
using WeatherApp.Services;

namespace WeatherApp.Repositories
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
        }

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Fetch API data, handle url strings and try catch
        public async Task<WeatherModel> GetWeatherAsync(string city)
        {
            try
            {
                var url = $"{AppConfig.WeatherApiBaseUrl}weather?q={city}&appid={AppConfig.WeatherApiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                //Built in method checks if status code not 200-299 and throws error
                response.EnsureSuccessStatusCode();

                //convert to Json
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<WeatherModel>(jsonResponse);

                return weatherData;
            } catch (HttpRequestException e)
            {
                Debug.WriteLine($"Error: {e.Message}");
                return null;
            } catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<ForecastItem>> GetForecastAsync(string city)
        {
            try
            {
                var url = $"{AppConfig.WeatherApiBaseUrl}forecast?q={city}&appid={AppConfig.WeatherApiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                //Built in method checks if status code not 200-299 and throws error
                response.EnsureSuccessStatusCode();

                //convert to Json
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<ForecastResponse>(jsonResponse, options);

                return weatherData.List;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"Error: {e.Message}");
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

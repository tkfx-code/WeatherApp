using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using WeatherApp.Model;
using WeatherApp.Services;
using static WeatherApp.Model.WeatherModelBase;

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

                if (response.IsSuccessStatusCode)
                {
                    //convert to read json
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = await response.Content.ReadFromJsonAsync<WeatherModel>(options);
                    return data;
                }
                return null;

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

                if (response.IsSuccessStatusCode)
                {
                    //convert to read json
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = await response.Content.ReadFromJsonAsync<ForecastResponse>(options);
                    return data?.List;
                }
                return null;
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

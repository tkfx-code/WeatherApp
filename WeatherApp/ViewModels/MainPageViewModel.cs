using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WeatherApp.Model;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    partial class MainPageViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;
        //Main Search object view
        public ObservableCollection<WeatherModel> Weather {  get; set; }
        public ObservableCollection<WeatherModel> Forecast { get; set; }
        //Detailed view
        [ObservableProperty]
        WeatherModel selectedWeather;
        [ObservableProperty] 
        private bool _isBusy;
        [ObservableProperty]
        private string _selectedCity;
        [ObservableProperty]
        private string _selectedTimeline;

        public RelayCommand ClearCommand { get; }

        public MainPageViewModel(IWeatherService weatherService)
        {
            //Repo
            _weatherService = weatherService;
            Weather = new ObservableCollection<WeatherModel>();
        }

        // Load Async method?
        // Update Dashboard method?

        //Properties, which variablesdo we send to UI? 
        [RelayCommand]
        public async void FetchCityWeather(string city)
        {
            _isBusy = true;
            _selectedCity = city;
            try
            {
                var result = await _weatherService.GetWeatherAsync(city);
                _isBusy = false;
                if (result != null)
                {
                    Weather.Clear();
                    Weather.Add(result);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
                return;
            }
        }
    }
}

using RTCoffeeAPI.Services.Interfaces;
using System.Text.Json;

namespace RTCoffeeAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _location; // e.g. "Melbourne,AU"

        public WeatherService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config[Utilitys.Constants.ConfigAPIKey] ?? throw new ArgumentNullException(Utilitys.Constants.APIKeyMissing); //Throw error
            _location = config[Utilitys.Constants.ConfigAPILocation] ?? Utilitys.Constants.DefaultLocation; //Default location set
        }

        public async Task<double> GetTemperatureAsync()
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={_location}&units=metric&appid={_apiKey}";
                using var resp = await _http.GetAsync(url);
                resp.EnsureSuccessStatusCode();

                using var doc = await JsonDocument.ParseAsync(await resp.Content.ReadAsStreamAsync());
                // JSON path: main.temp
                var temp = doc.RootElement
                              .GetProperty("main")
                              .GetProperty("temp")
                              .GetDouble();
                return temp;
            }
            catch (Exception ex)
            {
                //Log Exception here
                throw new Exception(ex.Message);
            }
            
        }
    }
}

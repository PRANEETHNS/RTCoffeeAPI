namespace RTCoffeeAPI.Services.Interfaces
{
    /// <summary>
    /// Returns the temperature
    /// </summary>
    public interface IWeatherService
    {      
        Task<double> GetTemperatureAsync();
        
    }
}

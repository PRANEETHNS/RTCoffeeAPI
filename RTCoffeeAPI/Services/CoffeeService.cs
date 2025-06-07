namespace RTCoffeeAPI.Services
{
    using Microsoft.AspNetCore.Http;
    using RTCoffeeAPI.Services.Interfaces;
    using System.Text.Json;
    using System.Threading.Tasks;
  
    public class CoffeeService : ICoffeeService
    {       
        private static int requestNum = 0;
        private readonly IWeatherService _weatherService;
        private readonly ILogger<CoffeeService> _log;

        public CoffeeService(IWeatherService weatherService, ILogger<CoffeeService> log)
        {  
            _weatherService = weatherService;
            _log = log;
        }

        public async Task<(int StatusCode, string? JsonBody)> BrewCoffeeAsync()
        {
            try
            {                
                // Validate if 1st April
                // Exit if validation true without checking other requirement
                var today = DateTimeOffset.Now;

#if DEBUG
                //Testing Purpose only for April Fools
                //today = new DateTime(DateTime.Now.Year, 4, 1);
#endif

                if (today.Month == 4 && today.Day == 1)
                    return await Task.FromResult((StatusCodes.Status418ImATeapot, (string?)null));


                // thread saftey introduced to ensure multiple coffee machine 
                // can use the API & increase call count by 1    
                // Every 5th call → 503 Service Unavailable
                if (Interlocked.Increment(ref requestNum) % 5 == 0)
                {
                    requestNum = 0; //Reset the request counter

                    //Out of coffee error.
                    return await Task.FromResult((StatusCodes.Status503ServiceUnavailable, (string?)null));
                }

                var msg = Utilitys.Constants.SuccessHotCoffMessage;

                //Additional function should not break the original code
                try
                {
                    //Check temperature and choose the message
                    var temp = await _weatherService.GetTemperatureAsync();
                    msg = temp > 30 ? Utilitys.Constants.SuccessColCofFMessage : Utilitys.Constants.SuccessHotCoffMessage;
                }
                catch (Exception ex)
                {
                    _log.LogError(Utilitys.Constants.WeatherServError);
                    _log.LogError(ex.InnerException.ToString());
                }               

                // Otherwise → 200 + JSON payload
                var payload = new
                {
                    message = msg,
                    prepared = today.ToString(Utilitys.Constants.RTDateTimeFormat)
                };

                var json = JsonSerializer.Serialize(payload);

                return await Task.FromResult((StatusCodes.Status200OK, json));
            }
            catch (Exception ex)
            {
                _log.LogError(Utilitys.Constants.BrewCoffeeError);
                throw new Exception(ex.Message);
            }
        }
    }
}

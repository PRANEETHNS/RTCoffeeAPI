namespace RTCoffeeAPI.Services
{
    using Microsoft.AspNetCore.Http;
    using RTCoffeeAPI.Services.Interfaces;
    using System.Text.Json;
    using System.Threading.Tasks;
  
    public class CoffeeService : ICoffeeService
    {       
        private static int requestNum = 0;

        public Task<(int StatusCode, string? JsonBody)> BrewCoffeeAsync()
        {
            // Validate if 1st April
            // Exit if validation true without checking other requirement
            var today = DateTimeOffset.Now;

#if DEBUG
            //Testing Purpose only for April Fools
            //today = new DateTime(DateTime.Now.Year, 4, 1);
#endif

            if (today.Month == 4 && today.Day == 1)
                return Task.FromResult((StatusCodes.Status418ImATeapot, (string?)null));


            // thread saftey introduced to ensure multiple coffee machine 
            // can use the API & increase call count by 1    
            // Every 5th call → 503 Service Unavailable
            if (Interlocked.Increment(ref requestNum) % 5 == 0)
            {
                requestNum = 0; //Reset the request counter

                //Out of coffee error.
                return Task.FromResult((StatusCodes.Status503ServiceUnavailable, (string?)null));
            }

            // Otherwise → 200 + JSON payload
            var payload = new
            {
                message = Utilitys.Constants.SuccessMessage,
                prepared = today.ToString(Utilitys.Constants.RTDateTimeFormat)
            };

            var json = JsonSerializer.Serialize(payload);

            return Task.FromResult((StatusCodes.Status200OK, json));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RTCoffeeAPI.Services;
using RTCoffeeAPI.Services.Interfaces;


namespace RTCoffeeAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/brew-coffee")]
    public class CoffeeController : ControllerBase
    {
        private readonly ICoffeeService _coffeeService;
        private readonly ILogger<CoffeeService> _log;

        public CoffeeController(ICoffeeService coffeeService, ILogger<CoffeeService> log)
        {
            _coffeeService = coffeeService;
            _log = log;
        }

        /// <summary>
        /// HTTPGET for getting Brewed Coffee V1
        /// </summary>
        /// <returns>Returns either 200 / 418 / 503</returns>       
        [HttpGet]        
        public async Task<IActionResult> GetBrewCoffee()
        {
            try
            {
                var (status, jsonBody) = await _coffeeService.BrewCoffeeAsync();

                if (status == StatusCodes.Status200OK && jsonBody is not null)
                {
                    // Return JSON with 200 OK
                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status200OK,
                        ContentType = "application/json",
                        Content = jsonBody
                    };
                }
                else
                {
                    // Return 418 or 503 with empty body
                    return StatusCode(status);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.InnerException.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }        
    }
}

namespace RTCoffeeAPI.Services.Interfaces
{
    /// <summary>
    /// Interface for CoffeeService
    /// </summary>
    public interface ICoffeeService
    {
        /// <summary>
        /// The request for BrewCoffee handled
        /// </summary>
        /// <returns>Response code & JSON body</returns>
        Task<(int StatusCode, string? JsonBody)> BrewCoffeeAsync();
    }
}

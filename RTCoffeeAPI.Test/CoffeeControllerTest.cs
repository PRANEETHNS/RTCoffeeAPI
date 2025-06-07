using RTCoffeeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RTCoffeeAPI.Controllers;
using RTCoffeeAPI.Services;
using System.Text.Json;
using Microsoft.Extensions.Logging;
namespace RTCoffeeAPI.Test
{
    public class CoffeeControllerTest
    {

        [Fact]
        public async Task GetBrewCoffee_ServiceReturns200_ReturnsStatusCode200()
        {
            // Arrange
            var expectedJson = "{\"message\":\"Your piping hot coffee is ready\",\"prepared\":\"2025-06-07T10:00:00+10:00\"}";
            var mockService = new Mock<ICoffeeService>();
            mockService
                .Setup(s => s.BrewCoffeeAsync())
                .ReturnsAsync((StatusCodes.Status200OK, expectedJson));
            var loggerMock = new Mock<ILogger<CoffeeService>>();
            var controller = new CoffeeController(mockService.Object, loggerMock.Object);

            // Act
            var result = await controller.GetBrewCoffee();

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal(StatusCodes.Status200OK, contentResult.StatusCode);
            Assert.Equal("application/json", contentResult.ContentType);
            Assert.Equal(expectedJson, contentResult.Content);
        }

        [Fact]
        public async Task GetBrewCoffee_ServiceReturns503_ReturnsStatusCode503()
        {
            // Arrange
            var mockService = new Mock<ICoffeeService>();
            mockService
                .Setup(s => s.BrewCoffeeAsync())
                .ReturnsAsync((StatusCodes.Status503ServiceUnavailable, (string?)null));
            var loggerMock = new Mock<ILogger<CoffeeService>>();
            var controller = new CoffeeController(mockService.Object, loggerMock.Object);

            // Act
            var result = await controller.GetBrewCoffee();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status503ServiceUnavailable, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetBrewCoffee_ServiceReturns418_ReturnsStatusCode418()
        {
            // Arrange
            var mockService = new Mock<ICoffeeService>();
            mockService
                .Setup(s => s.BrewCoffeeAsync())
                .ReturnsAsync((StatusCodes.Status418ImATeapot, (string?)null));
            var loggerMock = new Mock<ILogger<CoffeeService>>();
            var controller = new CoffeeController(mockService.Object, loggerMock.Object);

            // Act
            var result = await controller.GetBrewCoffee();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status418ImATeapot, statusResult.StatusCode);
        }

        [Theory]
        [InlineData(30.0, "Your piping hot coffee is ready")]       
        [InlineData(25.5, "Your piping hot coffee is ready")]      
        [InlineData(30.1, "Your refreshing iced coffee is ready")] 
        [InlineData(45.0, "Your refreshing iced coffee is ready")]
        public async Task BrewCoffee_WeatherBranchesCorrectly(double testTemp, string outputMsg)
        {                      
            var weatherMock = new Mock<IWeatherService>();
            weatherMock
                .Setup(w => w.GetTemperatureAsync())
                .ReturnsAsync(testTemp);
            var loggerMock = new Mock<ILogger<CoffeeService>>();
            var service = new CoffeeService(weatherMock.Object, loggerMock.Object);
                        
            var (status, json) = await service.BrewCoffeeAsync();

            Assert.Equal(StatusCodes.Status200OK, status);
            Assert.False(string.IsNullOrEmpty(json));

            using var doc = JsonDocument.Parse(json);
            var msg = doc.RootElement.GetProperty("message").GetString();
            Assert.Equal(outputMsg, msg);
        }
    }
}

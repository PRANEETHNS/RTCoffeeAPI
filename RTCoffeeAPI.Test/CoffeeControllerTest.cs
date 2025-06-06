using RTCoffeeAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RTCoffeeAPI.Controllers;
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

            var controller = new CoffeeController(mockService.Object);

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

            var controller = new CoffeeController(mockService.Object);

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

            var controller = new CoffeeController(mockService.Object);

            // Act
            var result = await controller.GetBrewCoffee();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status418ImATeapot, statusResult.StatusCode);
        }
    }
}
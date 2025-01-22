using Xunit;
using Moq;
using Employee.API.Controllers;
using Employee.Application.Interfaces;
using Employee.Application.Models;
using Employee.Domain.SeedWork;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Employee.API.Controllers.Tests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<INotification> _notificationMock;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _notificationMock = new Mock<INotification>();
            _controller = new EmployeeController(_employeeServiceMock.Object, _notificationMock.Object);
        }

        [Fact]
        public async Task Post_ValidEmployee_ReturnsOk()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest
            {
                EmployeeID = 1234,
                EmployeeName = "John Doe",
                EmployeeAge = 30,
                EmployeeAddress = "123 Main St"
            };
            var employeeResponse = new EmployeeResponse { Message = "Employee record has been added successfully." };
            _employeeServiceMock.Setup(service => service.AddAsync(employeeRequest)).ReturnsAsync(employeeResponse);

            // Act
            var result = await _controller.Post(employeeRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(employeeResponse, okResult.Value);
        }

        [Fact]
        public async Task Post_InvalidEmployee_ReturnsBadRequest()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest
            {
                EmployeeID = 12, // Invalid ID
                EmployeeName = "John Doe with a very long name exceeding twenty characters",
                EmployeeAge = 300, // Invalid age
                EmployeeAddress = "123 Main St"
            };
            var errorMessage = "Invalid EmployeeID. It must be a 4-digit number."; // Assume multiple, but example
            var employeeResponse = new EmployeeResponse { Message = errorMessage };
            _employeeServiceMock.Setup(service => service.AddAsync(employeeRequest)).ReturnsAsync(employeeResponse);

            // Act
            var result = await _controller.Post(employeeRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(employeeResponse, badRequestResult.Value);
        }
    }
}
using Xunit;
using Moq;
using Employee.Application.Services;
using Employee.Application.Interfaces;
using Employee.Application.Models;
using Employee.Domain.Entities;
using Employee.Infrastructure.Repository;
using Employee.Domain.SeedWork;
using System.Threading.Tasks;
using System.Linq;

namespace Employee.Application.Services.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<INotification> _notificationMock;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _notificationMock = new Mock<INotification>();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object, _notificationMock.Object);
        }

        [Fact]
        public async Task AddAsync_ValidEmployee_AddsEmployee()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest
            {
                EmployeeID = 1234,
                EmployeeName = "John Doe",
                EmployeeAge = 30,
                EmployeeAddress = "123 Main St"
            };
            _notificationMock.Setup(n => n.HasNotifications).Returns(false);

            // Act
            var response = await _employeeService.AddAsync(employeeRequest);

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.AddAsync(It.Is<EmployeeRecord>(e =>
                e.EmployeeID == 1234 &&
                e.EmployeeName == "John Doe" &&
                e.EmployeeAge == 30 &&
                e.EmployeeAddress == "123 Main St"
            )), Times.Once);
            Assert.Equal("Employee record has been added successfully.", response.Message);
        }

        [Fact]
        public async Task AddAsync_InvalidEmployeeID_ReturnsError()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest
            {
                EmployeeID = 12, // Invalid ID
                EmployeeName = "John Doe",
                EmployeeAge = 30,
                EmployeeAddress = "123 Main St"
            };
            _notificationMock.Setup(n => n.HasNotifications).Returns(true);
            _notificationMock.Setup(n => n.GetNotifications()).Returns(new[] { "Invalid EmployeeID. It must be a 4-digit number." });

            // Act
            var response = await _employeeService.AddAsync(employeeRequest);

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<EmployeeRecord>()), Times.Never);
            Assert.Equal("Invalid EmployeeID. It must be a 4-digit number.", response.Message);
        }

        [Fact]
        public async Task AddAsync_InvalidEmployeeName_ReturnsError()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest
            {
                EmployeeID = 1234,
                EmployeeName = "This name is definitely longer than twenty characters",
                EmployeeAge = 30,
                EmployeeAddress = "123 Main St"
            };
            _notificationMock.Setup(n => n.HasNotifications).Returns(true);
            _notificationMock.Setup(n => n.GetNotifications()).Returns(new[] { "Invalid EmployeeName. It must be up to 20 characters." });

            // Act
            var response = await _employeeService.AddAsync(employeeRequest);

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<EmployeeRecord>()), Times.Never);
            Assert.Equal("Invalid EmployeeName. It must be up to 20 characters.", response.Message);
        }

        [Fact]
        public async Task AddAsync_InvalidEmployeeAge_ReturnsError()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest
            {
                EmployeeID = 1234,
                EmployeeName = "John Doe",
                EmployeeAge = 300, // Invalid age
                EmployeeAddress = "123 Main St"
            };
            _notificationMock.Setup(n => n.HasNotifications).Returns(true);
            _notificationMock.Setup(n => n.GetNotifications()).Returns(new[] { "Invalid EmployeeAge. It must be a 2-digit number." });

            // Act
            var response = await _employeeService.AddAsync(employeeRequest);

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<EmployeeRecord>()), Times.Never);
            Assert.Equal("Invalid EmployeeAge. It must be a 2-digit number.", response.Message);
        }

        [Fact]
        public async Task AddAsync_InvalidEmployeeAddress_ReturnsError()
        {
            // Arrange
            var employeeRequest = new EmployeeRequest
            {
                EmployeeID = 1234,
                EmployeeName = "John Doe",
                EmployeeAge = 30,
                EmployeeAddress = "This address is definitely longer than thirty characters long."
            };
            _notificationMock.Setup(n => n.HasNotifications).Returns(true);
            _notificationMock.Setup(n => n.GetNotifications()).Returns(new[] { "Invalid EmployeeAddress. It must be up to 30 characters." });

            // Act
            var response = await _employeeService.AddAsync(employeeRequest);

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<EmployeeRecord>()), Times.Never);
            Assert.Equal("Invalid EmployeeAddress. It must be up to 30 characters.", response.Message);
        }
    }
}
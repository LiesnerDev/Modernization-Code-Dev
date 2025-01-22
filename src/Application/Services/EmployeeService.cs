using Employee.Application.Interfaces;
using Employee.Application.Models;
using Employee.Domain.Entities;
using Employee.Domain.SeedWork;
using Employee.Infrastructure.Repository;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Employee.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INotification _notification;

        public EmployeeService(IEmployeeRepository employeeRepository, INotification notification)
        {
            _employeeRepository = employeeRepository;
            _notification = notification;
        }

        public async Task<EmployeeResponse> AddAsync(EmployeeRequest employeeRequest)
        {
            Validate(employeeRequest);
            if (_notification.HasNotifications)
            {
                return new EmployeeResponse { Message = string.Join("; ", _notification.GetNotifications()) };
            }

            var employee = new EmployeeRecord
            {
                EmployeeID = employeeRequest.EmployeeID,
                EmployeeName = employeeRequest.EmployeeName,
                EmployeeAge = employeeRequest.EmployeeAge,
                EmployeeAddress = employeeRequest.EmployeeAddress
            };

            await _employeeRepository.AddAsync(employee);
            return "Employee record has been added successfully.";
        }

        private void Validate(EmployeeRequest request)
        {
            if (!Regex.IsMatch(request.EmployeeID.ToString(), "^\\d{4}$"))
            {
                _notification.AddNotification("Invalid EmployeeID. It must be a 4-digit number.");
            }

            if (string.IsNullOrWhiteSpace(request.EmployeeName) || request.EmployeeName.Length > 20)
            {
                _notification.AddNotification("Invalid EmployeeName. It must be up to 20 characters.");
            }

            if (!Regex.IsMatch(request.EmployeeAge.ToString(), "^\\d{2}$"))
            {
                _notification.AddNotification("Invalid EmployeeAge. It must be a 2-digit number.");
            }

            if (string.IsNullOrWhiteSpace(request.EmployeeAddress) || request.EmployeeAddress.Length > 30)
            {
                _notification.AddNotification("Invalid EmployeeAddress. It must be up to 30 characters.");
            }
        }
    }
}
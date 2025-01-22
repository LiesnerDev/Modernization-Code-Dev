using Employee.Application.Models;
using System.Threading.Tasks;

namespace Employee.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeResponse> AddAsync(EmployeeRequest employeeRequest);
    }
}
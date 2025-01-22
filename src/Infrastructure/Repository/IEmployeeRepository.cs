using Employee.Domain.Entities;
using System.Threading.Tasks;

namespace Employee.Infrastructure.Repository
{
    public interface IEmployeeRepository
    {
        Task AddAsync(EmployeeRecord employee);
    }
}
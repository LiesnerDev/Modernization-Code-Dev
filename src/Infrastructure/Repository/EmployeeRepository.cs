using Employee.Domain.Entities;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Employee.Infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _filePath = "EMPLOYEE.DAT";

        public async Task AddAsync(EmployeeRecord employee)
        {
            var json = JsonSerializer.Serialize(employee);
            using (var writer = new StreamWriter(_filePath, append: true))
            {
                await writer.WriteLineAsync(json);
            }
        }
    }
}
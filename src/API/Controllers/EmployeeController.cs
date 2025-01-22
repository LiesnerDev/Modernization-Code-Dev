using Microsoft.AspNetCore.Mvc;
using Employee.Application.Interfaces;
using Employee.Application.Models;
using Employee.Domain.SeedWork;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService, INotification notification) : base(notification)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeRequest employeeRequest)
        {
            return Response(await _employeeService.AddAsync(employeeRequest));
        }
    }
}
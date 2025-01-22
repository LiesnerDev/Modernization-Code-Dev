using Employee.Application.Interfaces;
using Employee.Application.Services;
using Employee.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Employee.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            // Add other services here
            return services;
        }
    }
}
namespace Employee.Application.Models
{
    public class EmployeeRequest
    {
        public int EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public int EmployeeAge { get; set; }
        public string? EmployeeAddress { get; set; }
    }
}
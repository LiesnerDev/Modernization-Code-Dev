namespace Employee.Application.Models
{
    public class EmployeeResponse : BaseResponse
    {
        public string Message { get; set; }

        public static implicit operator EmployeeResponse(string message)
        {
            return new EmployeeResponse { Message = message };
        }
    }
}
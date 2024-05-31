using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Basic.Models
{
    public class Employee
    {
        public int Id { get; set; } = 0;
        public string FName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string DepartmentId { get; set; } = string.Empty;
        public string Postion { get; set; } = string.Empty;

        public double Salary { get; set; } = 0.0;
        public string HireDate { get; set; } = string.Empty;

        public string IDProofTypeId { get; set; } = string.Empty;

    }
}

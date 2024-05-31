using Basic.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.Intrinsics.Arm;


namespace Basic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly string _connectionString;
        
        public EmployeeController(IConfiguration configuration)   //is used to read settings and connection string as in we can directly access Json property through I configuration 
        {
            _connectionString = configuration.GetConnectionString("defaultconnection");
        }


        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee()
        {
            
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<Employee>("usp_GetEmployees", commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
        }

        [HttpPost("InsertEmployee")]
        public IActionResult InsertEmployee(Employee emp)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Execute("usp_InsertEmployee",
                new { emp.FName, emp.Email, emp.DepartmentId, emp.Postion, emp.Salary, emp.HireDate },
                commandType: CommandType.StoredProcedure);
                return Ok("Inserted successfully.");
            }
            
        }

        [HttpGet("{name}")]
        public IActionResult GetEmployeeById(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Fname, Email, DepartmentId, Postion, Salary, HireDate, IDProofTypeId FROM tblEmployees WHERE FName = @FName";
                var employee = connection.QueryFirstOrDefault<Employee>(query, new { FName=name });

                if (employee != null)
                {
                    return Ok(employee);
                }
                else
                {
                    return NotFound("Employee not found."); 
                }
            }
        }

        [HttpDelete("DeleteEmployee")]
        public IActionResult DeleteDepartment(string FName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Execute("usp_DeleteEmployee",
                new { FName = FName },
                           commandType: CommandType.StoredProcedure);
            }
            return Ok("Deleted successfully");
        }

        [HttpPost("UpdateEmployee")]
        public IActionResult UpdateDepartment(Employee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Execute("usp_UpdateEmployee", new { Id = employee.Id, Email= employee.Email, FName = employee.FName, DepartmentID = employee.DepartmentId, Postion = employee.Postion, Salary =employee.Salary }, commandType: CommandType.StoredProcedure);
            }
            return Ok("Updated Details");   
        }

        [HttpPost("UploadImage")]
        public IActionResult UploadImage(IFormFile file)   // IForm file is a sepcified variable for getting files from 
        {
            return Ok(new ImageHandler().Upload(file));
                
        }
    
    }
}





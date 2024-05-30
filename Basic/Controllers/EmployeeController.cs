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
        //string _connectionstring = Configuration["DefaultConnection:Data Source=172.16.18.15;Initial Catalog=EmployeeManagement;MultipleActiveResultSets=True;User ID=hbits-mihir;Password=lwC655E00lZh"];
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
                new { emp.Id, emp.Name , emp.Email, emp.DepartmentId, emp.Postion, emp.Salary, emp.HireDate },
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

        [HttpPost]
        public IActionResult UploadImage(IFormFile file)   // IForm file is a sepcified variable for getting files from 
        {
            return Ok(new ImageHandler().Upload(file));
                
        }
    
    }
}
//private readonly string _connectionString;

//public EmployeeController()
//{
//    _connectionString = "Data Source=172.16.18.15;Initial Catalog=EmployeeManagement;MultipleActiveResultSets=True;TrustServerCertificate=True;Uid=hbits-mihir;password=lwC655E00lZh";
//}


//[HttpPost]
//public IActionResult SaveUserImage(int userId, string imagePath)
//{
//    using (var connection = new SqlConnection(_connectionString))
//    {
//        connection.Open();

//        // SQL command to update user table with the image path
//        string query = "UPDATE Users SET ImagePath = @ImagePath WHERE UserId = @UserId";

//        // Creating SQL command with parameters
//        using (var command = new SqlCommand(query, connection))
//        {
//            // Adding parameters
//            command.Parameters.AddWithValue("@ImagePath", imagePath);
//            command.Parameters.AddWithValue("@UserId", userId);

//            // Executing the command
//            int rowsAffected = command.ExecuteNonQuery();

//            if (rowsAffected > 0)
//            {
//                return Ok("Image path saved successfully");
//            }
//            else
//            {
//                return BadRequest("Failed to save image path");
//            }
//        }
//    }
//}


//[HttpGet]
//public string GetEmployees()
//{
//    return "mihir";
//}

//[HttpGet("myemp")]
//public string GetMyEmp(int i)
//{
//    return i.ToString();
//}

//[HttpGet("GetEmployee")]
//public IActionResult GetEmployee(int id)
//{
//    if (id > 4)
//    {
//        return Ok(1);
//    }

//    else
//    {
//        return NotFound();
//    }


//}
//[HttpPost]
//public string Post(Employee employee)
//{
//    string a = employee.Name;
//    return a;
//}



// return all data of employee
//[HttpGet]
//public async Task<IEnumerable<Employee>> GetEmployees()
//{
//    //var arar = await _employeeRepo.GetEmployees();
//    //return Ok(arar);
//    return await _employeeRepo.GetEmployees();

//}



// return data of employee that is specified by id

//[HttpGet("{id:int}")]
//    public async Task<IActionResult> GetEmployee(int Id)
//    {
//        var employee = await _employeeRepo.GetEmployeeById(Id);
//        if (employee == null)
//        {
//            return NotFound();
//        }
//        return Ok(employee);
//    }

// add employee karega !!

//[HttpPost]
//public async Task<IActionResult> PostEmployee(Employee employee)
//{
//    if (employee == null)
//    {
//        return BadRequest();
//    }
//    await _employeeRepo.AddEmployee(employee);
//    return Ok("added");

//}

// delete employee method

//[HttpDelete("{id}")]
//public async Task<IActionResult> DeleteEmployee(int id)
//{
//    var employee = _employeeRepo.GetEmployeeById(id);
//    if (employee == null)
//    {  return NotFound(); }

//}





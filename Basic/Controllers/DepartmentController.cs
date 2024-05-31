using Basic.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace Basic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        


        private readonly string _connectionString;

        public DepartmentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("GetDepartment")]
        public IActionResult GetDepartments()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var SqlCommand = new SqlCommand())
            {
                connection.Open();
                var data12 = connection.Query<Department>("usp_GetDepartments", commandType: CommandType.StoredProcedure);
                return Ok(data12);
            }
        }

        [HttpPost]
        public IActionResult InsertDepartment(Department dep)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                        var result = connection.Execute("usp_InsertDepartment",
                            new { dep.Id, dep.Name, dep.Location },
                            commandType: CommandType.StoredProcedure);

                        return Ok(result);
                    }
                }

        [HttpDelete("DeleteDepartment")]
        public IActionResult DeleteDepartment(int departmentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Execute("usp_DeleteDepartment",
                new { Id = departmentId },
                           commandType: CommandType.StoredProcedure);
            }
            return Ok("Deleted successfully");
        }

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment(Department department)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Execute("usp_Updatedepartment", new { Id = department.Id, Name = department.Name, Location = department.Location }, commandType: CommandType.StoredProcedure);
            }
            return Ok("Updated Details");
        }
    }
}

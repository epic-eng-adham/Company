using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Data;
using TestAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext db;
        public EmployeeController(AppDbContext db) {this.db = db;}
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            List<Employee> employees= await db.employees.ToListAsync();
            return Ok(employees);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            Employee employee =await db.employees.SingleOrDefaultAsync(p=>p.Id==id);
            return Ok(employee);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee()
        {
            var employee=await Request.ReadFromJsonAsync<Employee>();
            await db.employees.AddAsync(employee);
            await db.SaveChangesAsync();
            var employees=await db.employees.ToListAsync();
            return Ok(employees);
        }
        [HttpPatch]
        public async Task<IActionResult> EditEmployee(int id, [FromBody]JsonPatchDocument<Employee> employee)
        {
           var data = await db.employees.Include(p => p.Department).SingleOrDefaultAsync(p=>p.Id==id);
            if(data==null)
                return NotFound("Employee not found");
            employee.ApplyTo(data);
            await db.SaveChangesAsync();
            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            Employee employee = await db.employees.SingleOrDefaultAsync(p => p.Id == id);
            if (employee == null)
                return NotFound("Employee not found");
            db.employees.Remove(employee);
            await db.SaveChangesAsync();
            return Ok("Employee deleted successfully");
        }
    }
}

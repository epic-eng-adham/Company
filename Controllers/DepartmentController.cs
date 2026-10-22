using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using TestAPI.Data;
using TestAPI.Model;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext db;
        public DepartmentController(AppDbContext db) { this.db = db; }
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            List<Department> departments=await db.departments.ToListAsync();
            return Ok(departments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartemnetById(int id)
        {
            Department department = await db.departments.SingleOrDefaultAsync(p=>p.Id==id);
            return Ok(department);
        }
        [HttpPost]
        public async Task<IActionResult> AddDepartment()
        {
            var dep = await Request.ReadFromJsonAsync<Department>();
            await db.departments.AddAsync(dep);
            await db.SaveChangesAsync();
            var departments =await db.departments.ToListAsync();
            return Ok(departments);
        }
        [HttpPatch]
        public async Task<IActionResult> EditDepartment(int id, [FromBody]JsonPatchDocument<Department> dept)
        {
            var data =await db.departments.SingleOrDefaultAsync(p=>p.Id==id);
            if (data == null)
                return NotFound("Department not found");
            dept.ApplyTo(data);
            await db.SaveChangesAsync();
            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            Department department = await db.departments.SingleOrDefaultAsync(p=>p.Id==id);
            db.departments.Remove(department);  
            await db.SaveChangesAsync();
            return Ok("Department Deleted successfully");
        }
    }
}

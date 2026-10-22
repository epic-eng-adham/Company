using Microsoft.EntityFrameworkCore;
using TestAPI.Model;

namespace TestAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<Department> departments {  get; set; }
        public DbSet<Employee> employees { get; set; }
    }
}

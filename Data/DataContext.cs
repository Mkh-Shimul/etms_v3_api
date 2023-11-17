using Microsoft.EntityFrameworkCore;
using ETMS_API.Models;

namespace ETMS_API.Data
{
	public class DataContext : DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Bus> Buses { get; set; }
    }
}

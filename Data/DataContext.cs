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
        public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<Menu> Menus { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasKey(u => u.Id);

			modelBuilder.Entity<UserRole>()
				.HasKey(r => r.Id);

			// Configure the relationship between User and UserRole
			modelBuilder.Entity<User>()
				.HasOne(u => u.UserRole)
				.WithMany(r => r.Users)
				.HasForeignKey(u => u.UserRoleId)
				.OnDelete(DeleteBehavior.Restrict); // Specify the desired delete behavior if needed

			// Configure many-to-many relationship between UserRole and Menu
			modelBuilder.Entity<UserRole>()
				.HasMany(ur => ur.Menus)
				.WithMany(m => m.UserRoles)
				.UsingEntity(j => j.ToTable("MenuPermission")); // You can customize the join table name if needed


		}
	}
}

using System.ComponentModel.DataAnnotations;

namespace ETMS_API.Models
{
	public class Menu
	{
		[Key]
		public int Id { get; set; }
		public string MenuName { get; set; } = string.Empty;
		public string MenuUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
	}
}

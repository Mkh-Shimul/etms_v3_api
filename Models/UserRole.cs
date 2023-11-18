using System.ComponentModel.DataAnnotations;

namespace ETMS_API.Models
{
	public class UserRole
	{
		[Key]
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdateAt { get; set; }
        public DateTime? UpdatedBy { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETMS_API.Models
{
	public class User
	{
        [Key]
        public int Id { get; set; }
        [StringLength(500), Required]
        public string FullName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string UserName { get; set; } = string.Empty;
		public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string PasswordInPlainText { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsEmployee { get; set; } = false;
        public int? CreateBy { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
		public int? UserRoleId { get; set; } // Foreign key


        [ForeignKey("UserRoleId")]
        public UserRole? UserRole { get; set; }  
	}
}

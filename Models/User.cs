using System.ComponentModel.DataAnnotations;

namespace ETMS_API.Models
{
	public class User
	{
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
		public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string PasswordInPlainText { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
        public int? UpdateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

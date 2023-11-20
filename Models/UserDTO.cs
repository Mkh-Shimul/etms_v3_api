namespace ETMS_API.Models
{
	public class UserDTO
	{
		public string FullName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public int UserRoleId { get; set; }
		public bool IsActive { get; set; }
	}
}

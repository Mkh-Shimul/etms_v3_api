using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ETMS_API.Models
{
	public class Employee
	{
		[Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
		[Required, EmailAddress]
        public string Email { get; set; }
        public int Designation { get; set; }
        public int Gender { get; set; }
        public bool IsActive { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

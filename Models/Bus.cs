using System.ComponentModel.DataAnnotations;

namespace ETMS_API.Models
{
	public class Bus
	{
		[Key]
        public int Id { get; set; }
        public string BusNumber { get; set; } = string.Empty;
        public string StartFrom { get; set; } = string.Empty;
        public string StopAt { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

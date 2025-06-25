using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grp_management.Models
{
    public class GroupEmployee
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
} 
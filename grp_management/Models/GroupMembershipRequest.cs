using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grp_management.Models
{
    public class GroupMembershipRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public string? AdminComments { get; set; }

        public int? ProcessedByUserId { get; set; }
        [ForeignKey("ProcessedByUserId")]
        public User? ProcessedByUser { get; set; }

        public DateTime? ProcessedDate { get; set; }

        // Navigation properties
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }
    }
} 
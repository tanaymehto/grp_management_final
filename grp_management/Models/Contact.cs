using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grp_management.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        // Make EmployeeId nullable
        public int? EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string AccessLevel { get; set; } = "Member";

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        // Optional: Link to the User who is associated with this contact entry
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
} 
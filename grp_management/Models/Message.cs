using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grp_management.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string SentVia { get; set; } = "Email"; // Email, SMS, WhatsApp

        [Required]
        public string MessageContent { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Sent, Failed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }

        // Navigation properties
        [ForeignKey("GroupId")]
        public Group? Group { get; set; }
    }
} 
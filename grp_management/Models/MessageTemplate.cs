using System.ComponentModel.DataAnnotations;

namespace grp_management.Models
{
    public class MessageTemplate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = "General"; // General, Frequent, Custom

        public string? Placeholders { get; set; } // JSON string of available placeholders

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
} 
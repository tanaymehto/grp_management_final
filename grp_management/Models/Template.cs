using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace grp_management.Models
{
    public class Template
    {
        [Key]
        public int TemplateID { get; set; }

        [Required]
        [StringLength(100)]
        public string TemplateName { get; set; } = string.Empty;

        [Required]
        public string TemplateMsg { get; set; } = string.Empty;

        [Required]
        public string TemplateType { get; set; } = string.Empty; // "General" or "Frequent"

        public string? Placeholders { get; set; } // JSON string to store placeholder information

        public ICollection<SentMsg> SentMessages { get; set; } = new List<SentMsg>();
    }
} 
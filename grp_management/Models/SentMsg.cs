using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grp_management.Models
{
    public class SentMsg
    {
        [Key]
        public int SentMsgID { get; set; }

        [Required]
        public string MessageContent { get; set; } = string.Empty;

        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string SentVia { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        public int? TemplateID { get; set; }
        [ForeignKey("TemplateID")]
        public Template? Template { get; set; }

        public string? VariablesJson { get; set; } // Stores variables as JSON string

        // Foreign key to Employee who sent the message
        public int SenderEmployeeId { get; set; }
        [ForeignKey("SenderEmployeeId")]
        public Employee? Sender { get; set; }
    }
} 
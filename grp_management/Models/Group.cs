using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace grp_management.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // New properties for admin controls
        public bool IsPrivate { get; set; } = false;
        
        public int? MaxMembers { get; set; }
        
        [StringLength(100)]
        public string? DepartmentRestriction { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        public int? CreatedByUserId { get; set; }
        
        [ForeignKey("CreatedByUserId")]
        public User? CreatedByUser { get; set; }

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<SentMsg> SentMessages { get; set; } = new List<SentMsg>();
        public ICollection<GroupMembershipRequest> MembershipRequests { get; set; } = new List<GroupMembershipRequest>();

        // Navigation properties
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<GroupEmployee> GroupEmployees { get; set; } = new List<GroupEmployee>();
    }
} 
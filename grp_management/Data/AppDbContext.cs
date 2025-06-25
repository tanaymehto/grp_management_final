using Microsoft.EntityFrameworkCore;
using grp_management.Models;
using System.Text.Json;
using BCrypt.Net;

namespace grp_management.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<MessageTemplate> MessageTemplates { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<GroupEmployee> GroupEmployees { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<GroupMembershipRequest> GroupMembershipRequests { get; set; } = null!;
        public DbSet<Template> Templates { get; set; } = null!;
        public DbSet<SentMsg> SentMsgs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Group>()
                .HasIndex(g => g.Name)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Configure many-to-many relationship between Group and Employee
            modelBuilder.Entity<GroupEmployee>()
                .HasKey(ge => new { ge.GroupId, ge.EmployeeId });

            modelBuilder.Entity<GroupEmployee>()
                .HasOne(ge => ge.Group)
                .WithMany(g => g.GroupEmployees)
                .HasForeignKey(ge => ge.GroupId);

            modelBuilder.Entity<GroupEmployee>()
                .HasOne(ge => ge.Employee)
                .WithMany(e => e.GroupEmployees)
                .HasForeignKey(ge => ge.EmployeeId)
                .HasPrincipalKey(e => e.EmpNO);

            // Configure Group entity
            modelBuilder.Entity<Group>()
                .HasOne(g => g.CreatedByUser)
                .WithMany()
                .HasForeignKey(g => g.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Contact entity
            modelBuilder.Entity<Contact>()
                .Property(c => c.AccessLevel)
                .HasDefaultValue("Member");

            // Configure GroupMembershipRequest entity
            modelBuilder.Entity<GroupMembershipRequest>()
                .Property(r => r.Status)
                .HasDefaultValue("Pending");

            // Configure GroupMembershipRequest entity's relationship to Employee
            modelBuilder.Entity<GroupMembershipRequest>()
                .HasOne(r => r.Employee)
                .WithMany() // Assuming Employee doesn't have a collection of requests
                .HasForeignKey(r => r.EmployeeId)
                .HasPrincipalKey(e => e.EmpNO); // Explicitly specify the principal key

            // Configure SentMsg entity's relationship to Employee
            modelBuilder.Entity<SentMsg>()
                .HasOne(s => s.Sender)
                .WithMany() // An employee can send many messages
                .HasForeignKey(s => s.SenderEmployeeId)
                .HasPrincipalKey(e => e.EmpNO) // Employee's PK is EmpNO
                .OnDelete(DeleteBehavior.Cascade); // Or Restrict, depending on desired behavior

            // Configure Message entity
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Group)
                .WithMany(g => g.Messages)
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure User entity
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);

            // Seed test data
            SeedTestData(modelBuilder);
        }

        private void SeedTestData(ModelBuilder modelBuilder)
        {
            // Seed test employees with negative IDs
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmpNO = -1,
                    Name = "Test Employee 1",
                    Email = "test.employee1@example.com",
                    Department = "IT",
                    Position = "Developer",
                    CreatedAt = DateTime.UtcNow
                },
                new Employee
                {
                    EmpNO = -2,
                    Name = "Test Employee 2",
                    Email = "test.employee2@example.com",
                    Department = "HR",
                    Position = "Assistant",
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed test groups with negative IDs
            modelBuilder.Entity<Group>().HasData(
                new Group
                {
                    Id = -1,
                    Name = "Test Group 1",
                    Description = "A test group for membership requests",
                    IsPrivate = true,
                    MaxMembers = 10,
                    DepartmentRestriction = "IT",
                    CreatedAt = DateTime.UtcNow
                },
                new Group
                {
                    Id = -2,
                    Name = "Test Group 2",
                    Description = "Another test group",
                    IsPrivate = false,
                    MaxMembers = 20,
                    DepartmentRestriction = "HR",
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed test membership requests (using the negative employee and group IDs)
            modelBuilder.Entity<GroupMembershipRequest>().HasData(
                new GroupMembershipRequest
                {
                    RequestId = -1,
                    GroupId = -1,
                    EmployeeId = -1,
                    RequestDate = DateTime.UtcNow.AddDays(-1),
                    Status = "Pending"
                },
                new GroupMembershipRequest
                {
                    RequestId = -2,
                    GroupId = -2,
                    EmployeeId = -2,
                    RequestDate = DateTime.UtcNow,
                    Status = "Pending"
                },
                new GroupMembershipRequest
                {
                    RequestId = -3,
                    GroupId = -1,
                    EmployeeId = -2,
                    RequestDate = DateTime.UtcNow.AddDays(-2),
                    Status = "Approved",
                    ProcessedDate = DateTime.UtcNow.AddDays(-1)
                },
                new GroupMembershipRequest
                {
                    RequestId = -4,
                    GroupId = -2,
                    EmployeeId = -1,
                    RequestDate = DateTime.UtcNow.AddDays(-3),
                    Status = "Rejected",
                    ProcessedDate = DateTime.UtcNow.AddDays(-2),
                    AdminComments = "Did not meet department requirements."
                }
            );

            // Seed test Users - Only add if they don't already exist
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = -1,
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password", 12),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = -2,
                    Username = "user",
                    Email = "user@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password", 12),
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed test Contacts
            modelBuilder.Entity<Contact>().HasData(
                new Contact
                {
                    Id = -1,
                    GroupId = -1,
                    EmployeeId = -1,
                    UserName = "testuser1", 
                    UserId = -1,
                    AccessLevel = "Admin",
                    AddedAt = DateTime.UtcNow
                },
                new Contact
                {
                    Id = -2,
                    GroupId = -2,
                    EmployeeId = -2,
                    UserName = "testuser2", 
                    UserId = -2,
                    AccessLevel = "Member",
                    AddedAt = DateTime.UtcNow
                }
            );

            // Seed test SentMsgs
            modelBuilder.Entity<SentMsg>().HasData(
                new SentMsg
                {
                    SentMsgID = -1,
                    GroupId = -1,
                    TemplateID = -1,
                    MessageContent = "This is a test message.",
                    VariablesJson = "{}",
                    SentDate = DateTime.UtcNow.AddHours(-10),
                    Status = "Sent",
                    SentVia = "WebApp",
                    SenderEmployeeId = -1 
                },
                new SentMsg
                {
                    SentMsgID = -2,
                    GroupId = -2,
                    TemplateID = -2,
                    MessageContent = "This is another test message.",
                    VariablesJson = "{}",
                    SentDate = DateTime.UtcNow.AddHours(-5),
                    Status = "Sent",
                    SentVia = "WebApp",
                    SenderEmployeeId = -2
                },
                new SentMsg
                {
                    SentMsgID = -3,
                    GroupId = -1,
                    TemplateID = -1,
                    MessageContent = "A third test message for all.",
                    VariablesJson = "{}",
                    SentDate = DateTime.Parse("2025-06-21T19:04:57.4854134Z"),
                    Status = "Sent",
                    SentVia = "API",
                    SenderEmployeeId = -1
                }
            );

            // Seed test Templates
            modelBuilder.Entity<Template>().HasData(
                new Template
                {
                    TemplateID = -1,
                    TemplateName = "Happy Birthday",
                    TemplateType = "General",
                    TemplateMsg = "Happy Birthday, {{Name}}! Wishing you a wonderful day.",
                    Placeholders = JsonSerializer.Serialize(new Dictionary<string, string>
                    {
                        {"Name", "Recipient Name"}
                    })
                },
                new Template
                {
                    TemplateID = -2,
                    TemplateName = "Meeting Reminder",
                    TemplateType = "Frequent",
                    TemplateMsg = "Reminder: Meeting at 11 AM. See you there!",
                    Placeholders = "{}"
                },
                new Template
                {
                    TemplateID = -3,
                    TemplateName = "Project Update",
                    TemplateType = "General",
                    TemplateMsg = "Hello {{Name}}, this is an update regarding your project '{{ProjectName}}'. Current Status: {{Status}}.",
                    Placeholders = JsonSerializer.Serialize(new Dictionary<string, string>
                    {
                        {"Name", "Recipient Name"},
                        {"ProjectName", "Name of the Project"},
                        {"Status", "Current Project Status"}
                    })
                },
                new Template
                {
                    TemplateID = -4,
                    TemplateName = "Holiday Greeting",
                    TemplateType = "Frequent",
                    TemplateMsg = "Season's Greetings! Wishing you a happy and prosperous holiday season from all of us.",
                    Placeholders = "{}"
                },
                new Template
                {
                    TemplateID = -5,
                    TemplateName = "Office Maintenance",
                    TemplateType = "General",
                    TemplateMsg = "Please be advised that there will be scheduled maintenance in the office this weekend. Please save all your work before leaving on Friday.",
                    Placeholders = "{}"
                }
            );
        }
    }
} 
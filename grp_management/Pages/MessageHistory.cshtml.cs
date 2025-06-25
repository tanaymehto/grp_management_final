using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using grp_management.Models;
using grp_management.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace grp_management.Pages
{
    [Authorize]
    public class MessageHistoryModel : PageModel
    {
        private readonly AppDbContext _context;

        public MessageHistoryModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchGroup { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchSentVia { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchDate { get; set; }

        public List<MessageHistoryItem> Messages { get; set; } = new List<MessageHistoryItem>();

        public async Task OnGetAsync()
        {
            IQueryable<SentMsg> query = _context.SentMsgs.Include(m => m.Group);

            // If the user is not an admin, filter messages by their groups
            if (!User.IsInRole("Admin"))
            {
                var employeeIdClaim = User.FindFirstValue("EmployeeId");
                if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out var employeeId))
                {
                    // If no valid employee ID, show no messages
                    Messages = new List<MessageHistoryItem>();
                    return;
                }

                var userGroupIds = await _context.GroupEmployees
                    .Where(ge => ge.EmployeeId == employeeId)
                    .Select(ge => ge.GroupId)
                    .ToListAsync();

                query = query.Where(m => userGroupIds.Contains(m.GroupId) || m.SenderEmployeeId == employeeId);
            }

            // Apply filters if they are provided
            if (!string.IsNullOrWhiteSpace(SearchGroup))
            {
                query = query.Where(m => m.Group != null && m.Group.Name.Contains(SearchGroup));
            }

            if (!string.IsNullOrWhiteSpace(SearchMessage))
            {
                query = query.Where(m => m.MessageContent.Contains(SearchMessage));
            }

            if (!string.IsNullOrWhiteSpace(SearchStatus))
            {
                query = query.Where(m => m.Status == SearchStatus);
            }

            if (!string.IsNullOrWhiteSpace(SearchSentVia))
            {
                query = query.Where(m => m.SentVia == SearchSentVia);
            }

            if (SearchDate.HasValue)
            {
                var startDate = SearchDate.Value.Date;
                var endDate = startDate.AddDays(1);
                query = query.Where(m => m.SentDate >= startDate && m.SentDate < endDate);
            }

            var sentMsgs = await query
                .OrderByDescending(m => m.SentDate)
                .Select(m => new SentMsgDto {
                    SentDate = m.SentDate,
                    SentVia = m.SentVia,
                    GroupName = m.Group != null ? m.Group.Name : "Unknown",
                    MessageContent = m.Template != null ? m.Template.TemplateMsg : m.MessageContent,
                    VariablesJson = m.VariablesJson,
                    Status = m.Status
                })
                .ToListAsync();

            Messages = sentMsgs.Select(m => new MessageHistoryItem
                {
                    SentDate = m.SentDate,
                    SentVia = m.SentVia ?? "N/A",
                    SentTo = m.GroupName,
                    MessageTemplate = m.MessageContent,
                    Variables = !string.IsNullOrEmpty(m.VariablesJson) ? 
                                  JsonSerializer.Deserialize<Dictionary<string, string>>(m.VariablesJson) : 
                                  new Dictionary<string, string>(),
                    Status = m.Status ?? "Unknown"
                })
                .ToList();
        }
    }

    // DTO to avoid fetching the entire object and related entities
    public class SentMsgDto
    {
        public DateTime SentDate { get; set; }
        public string? SentVia { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public string? VariablesJson { get; set; }
        public string? Status { get; set; }
    }

    public class MessageHistoryItem
    {
        public DateTime SentDate { get; set; }
        public string SentVia { get; set; } = string.Empty;
        public string SentTo { get; set; } = string.Empty;
        public string MessageTemplate { get; set; } = string.Empty;
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public string Status { get; set; } = string.Empty;
    }
} 
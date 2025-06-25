using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using grp_management.Models;
using grp_management.Data;
using grp_management.Hubs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Antiforgery;
using System.Security.Claims;

namespace grp_management.Pages
{
    public class TemplatesModel : PageModel
    {
        private readonly ILogger<TemplatesModel> _logger;
        private readonly AppDbContext _context;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IAntiforgery _antiforgery;

        public TemplatesModel(
            ILogger<TemplatesModel> logger, 
            AppDbContext context, 
            IHubContext<MessageHub> hubContext,
            IAntiforgery antiforgery)
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
            _antiforgery = antiforgery;
        }

        public List<Template> GeneralTemplates { get; set; } = new List<Template>();
        public List<Template> FrequentTemplates { get; set; } = new List<Template>();
        public List<Group> AvailableGroups { get; set; } = new List<Group>();

        [BindProperty]
        public Dictionary<string, string?>? Placeholders { get; set; }

        [BindProperty]
        public int GroupId { get; set; }

        public async Task OnGetAsync()
        {
            if (!_context.Templates.Any())
            {
                var happyBirthdayTemplate = new Template
                {
                    TemplateName = "Happy Birthday (Fixed)",
                    TemplateMsg = "Happy birthday, {{Name}}",
                    TemplateType = "General",
                    Placeholders = JsonSerializer.Serialize(new Dictionary<string, string>
                    {
                        {"Name", "Recipient Name"}
                    })
                };
                var meeting = new Template
                {
                    TemplateName = "Meeting (Fixed)",
                    TemplateMsg = "Meeting at , {{Time}}",
                    TemplateType = "General",
                    Placeholders = JsonSerializer.Serialize(new Dictionary<string, string>
                    {
                        {"Time", "Time of meeting"}
                    })
                };

                var meetingReminderTemplate = new Template
                {
                    TemplateName = "Meeting Reminder (Fixed)",
                    TemplateMsg = "Just a reminder about our meeting at 11 am.",
                    TemplateType = "Frequent",
                    Placeholders = "{}"
                };

                var dynamicTemplate5 = new Template
                {
                    TemplateName = "Alert 1",
                    TemplateMsg = "Message from: NTPC SSTPS Message To: {{Group}} Message: {{Part1}} {{Part2}} {{Part3}} {{Part4}} {{Part5}}. NTPC Ltd.",
                    TemplateType = "General",
                    Placeholders = JsonSerializer.Serialize(new Dictionary<string, string>
                    {
                        {"Part1", "Blank 1"},
                        {"Part2", "Blank 2"},
                        {"Part3", "Blank 3"},
                        {"Part4", "Blank 4"},
                        {"Part5", "Blank 5"}
                    })
                };

                _context.Templates.AddRange(happyBirthdayTemplate, meetingReminderTemplate, dynamicTemplate5);
                await _context.SaveChangesAsync();
            }

            GeneralTemplates = await _context.Templates
                .Where(t => t.TemplateType == "General")
                .ToListAsync();

            FrequentTemplates = await _context.Templates
                .Where(t => t.TemplateType == "Frequent")
                .ToListAsync();

            var username = User.Identity.Name;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user != null && user.Role == "Admin")
            {
                // Admins can see all groups
            AvailableGroups = await _context.Groups.ToListAsync();
            }
            else if (user != null)
            {
                // Regular users only see groups they are members of
                var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (contact != null && contact.EmployeeId.HasValue)
                {
                    var employeeId = contact.EmployeeId.Value;
                    // Get GroupIDs from approved membership requests for the current user
                    var approvedGroupIds = await _context.GroupMembershipRequests
                        .Where(r => r.EmployeeId == employeeId && r.Status == "Approved")
                        .Select(r => r.GroupId)
                        .ToListAsync();
                    
                    AvailableGroups = await _context.Groups
                        .Where(g => approvedGroupIds.Contains(g.Id))
                        .ToListAsync();
                }
            }
        }

        public async Task<IActionResult> OnPostSendMessageAsync([FromBody] SendMessageRequest request)
        {
            try
            {
                _logger.LogInformation("OnPostSendMessageAsync called with TemplateID: {TemplateID}, GroupId: {GroupId}, Placeholders: {Placeholders}", 
                    request.TemplateID, request.GroupId, request.Placeholders != null ? string.Join(", ", request.Placeholders.Select(p => $"{p.Key}={p.Value}")) : "null");

                var template = await _context.Templates.FindAsync(request.TemplateID);
                if (template == null)
                {
                    _logger.LogWarning("Template with ID {TemplateID} not found.", request.TemplateID);
                    return new JsonResult(new { success = false, message = "Template not found" }) { StatusCode = 404 };
                }

                var group = await _context.Groups.FindAsync(request.GroupId);
                if (group == null)
                {
                    _logger.LogWarning("Group with ID {GroupId} not found.", request.GroupId);
                    return new JsonResult(new { success = false, message = "Group not found" }) { StatusCode = 400 };
                }

                var userName = User.Identity.Name;
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userName);
                if (user == null)
                {
                     return new JsonResult(new { success = false, message = "User not authenticated." }) { StatusCode = 401 };
                }

                var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (contact == null || contact.EmployeeId == null)
                {
                    return new JsonResult(new { success = false, message = "Could not find an employee associated with this user." }) { StatusCode = 404 };
                }
                var employeeId = contact.EmployeeId.Value;

                string message = template.TemplateMsg;
                _logger.LogInformation("Original template message: {Message}", message);

                // Replace {{Group}} placeholder with the actual group name
                message = message.Replace("{{Group}}", group.Name);

                // Replace other placeholders with provided values
                if (request.Placeholders != null)
                {
                    foreach (var placeholder in request.Placeholders)
                    {
                        if (string.IsNullOrEmpty(placeholder.Value))
                        {
                            _logger.LogWarning("Empty placeholder value for key: {Key}", placeholder.Key);
                            return new JsonResult(new { success = false, message = $"Please fill in all required fields" }) { StatusCode = 400 };
                        }
                        message = message.Replace("{{" + placeholder.Key + "}}", placeholder.Value);
                    }
                }

                _logger.LogInformation("Final message after replacements: {Message}", message);

                var sentMsg = new SentMsg
                {
                    MessageContent = message,
                    SentDate = DateTime.Now,
                    SentVia = request.SentVia,
                    Status = "Sent",
                    GroupId = request.GroupId,
                    TemplateID = request.TemplateID,
                    VariablesJson = request.Placeholders != null ? JsonSerializer.Serialize(request.Placeholders) : null,
                    SenderEmployeeId = employeeId
                };

                _context.SentMsgs.Add(sentMsg);
                await _context.SaveChangesAsync();

                // Notify all clients about the new message via SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveMessageUpdate", new
                {
                    sentDate = sentMsg.SentDate,
                    sentVia = sentMsg.SentVia,
                    sentTo = group.Name,
                    messageTemplate = template.TemplateMsg,
                    variables = request.Placeholders,
                    status = sentMsg.Status
                });

                _logger.LogInformation("Message sent successfully for TemplateID: {TemplateID}, GroupId: {GroupId}", request.TemplateID, request.GroupId);
                TempData["SuccessMessage"] = "Message sent successfully from template!";
                return new JsonResult(new { success = true, redirectUrl = Url.Page("/MessageHistory") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical Exception caught in OnPostSendMessageAsync. Inner Exception: {InnerException}", ex.InnerException?.Message);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return new JsonResult(new { success = false, message = $"Database error: {errorMessage}" }) { StatusCode = 500 };
            }
        }
    }

    public class SendMessageRequest
    {
        public int TemplateID { get; set; }
        public int GroupId { get; set; }
        public string SentVia { get; set; }
        public Dictionary<string, string>? Placeholders { get; set; } = new Dictionary<string, string>();
    }
}
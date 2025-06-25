using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using grp_management.Models;
using grp_management.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace grp_management.Pages
{
    public class SendMessageModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendMessageModel(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public SelectList Groups { get; set; }

        [BindProperty]
        public string? SelectedSendVia { get; set; }
        
        [BindProperty]
        public string MessageContent { get; set; }

        public class InputModel
        {
            [Required]
            public int GroupId { get; set; }

            [Required]
            public string MessageContent { get; set; } = string.Empty;

            [Required]
            public string SentVia { get; set; } = "Email"; // Default to Email
        }

        public async Task OnGetAsync()
        {
            List<Group> availableGroups = new List<Group>();
            var username = User.Identity.Name;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user != null && user.Role == "Admin")
            {
                availableGroups = await _context.Groups.ToListAsync();
            }
            else if (user != null)
            {
                var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (contact != null && contact.EmployeeId.HasValue)
                {
                    var employeeId = contact.EmployeeId.Value;
                    var approvedGroupIds = await _context.GroupMembershipRequests
                        .Where(r => r.EmployeeId == employeeId && r.Status == "Approved")
                        .Select(r => r.GroupId)
                        .ToListAsync();
                    
                    availableGroups = await _context.Groups
                        .Where(g => approvedGroupIds.Contains(g.Id))
                        .ToListAsync();
                }
            }
            
            var groupItems = availableGroups
                .OrderBy(g => g.Name)
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                }).ToList();

            groupItems.Insert(0, new SelectListItem { Value = "", Text = "-- Select Group --" });
            Groups = new SelectList(groupItems, "Value", "Text");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var employeeIdClaim = User.FindFirst("EmployeeId");
            if (employeeIdClaim == null || !int.TryParse(employeeIdClaim.Value, out var employeeId))
            {
                ModelState.AddModelError(string.Empty, "Could not identify the sender. Please log in again.");
                await LoadSelectListsAsync();
                return Page();
            }

            if (Input.GroupId == -1)
            {
                // Only Admins can send to all groups
                if (!User.IsInRole("Admin"))
                {
                    await LoadSelectListsAsync();
                    ModelState.AddModelError(string.Empty, "You are not authorized to send messages to all groups.");
                    return Page();
                }

                // Send to all groups
                var allGroups = await _context.Groups.ToListAsync();
                foreach (var group in allGroups)
                {
                    var sentMsg = new SentMsg
                    {
                        MessageContent = Input.MessageContent,
                        SentDate = DateTime.UtcNow,
                        SentVia = Input.SentVia,
                        Status = "Sent",
                        GroupId = group.Id,
                        SenderEmployeeId = employeeId
                    };
                    _context.SentMsgs.Add(sentMsg);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Message(s) sent successfully!";
                return RedirectToPage("/MessageHistory");
            }
            else
            {
                var sentMsg = new SentMsg
                {
                    MessageContent = Input.MessageContent,
                    SentDate = DateTime.UtcNow,
                    SentVia = Input.SentVia,
                    Status = "Sent",
                    GroupId = Input.GroupId,
                    SenderEmployeeId = employeeId
                };
                _context.SentMsgs.Add(sentMsg);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Message sent successfully!";
                return RedirectToPage("/MessageHistory");
            }
        }

        private async Task LoadSelectListsAsync()
        {
            var groupItems = await _context.Groups.OrderBy(g => g.Name).Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToListAsync();
                Groups = new SelectList(groupItems, "Value", "Text");
        }
    }
} 
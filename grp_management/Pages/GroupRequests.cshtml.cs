using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using grp_management.Data;
using grp_management.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace grp_management.Pages
{
    [Authorize(Roles = "Admin")]
    public class GroupRequestsModel : PageModel
    {
        private readonly AppDbContext _context;
        public List<GroupMembershipRequest> Requests { get; set; } = new();

        [BindProperty]
        public string? RejectionReason { get; set; }

        public GroupRequestsModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Requests = await _context.GroupMembershipRequests
                .Include(r => r.Employee)
                .Include(r => r.Group)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var req = await _context.GroupMembershipRequests.Include(r => r.Employee).Include(r => r.Group).FirstOrDefaultAsync(r => r.RequestId == id);
            if (req == null || req.Status != "Pending")
                return RedirectToPage();

            req.Status = "Approved";
            req.ProcessedDate = DateTime.UtcNow;
            req.ProcessedByUserId = GetCurrentUserId();
            // Add to Contacts
            if (req.Employee != null && req.Group != null)
            {
                _context.Contacts.Add(new Contact
                {
                    GroupId = req.Group.Id,
                    EmployeeId = req.Employee.EmpNO,
                    AccessLevel = "Member",
                    AddedAt = DateTime.UtcNow
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var req = await _context.GroupMembershipRequests.FirstOrDefaultAsync(r => r.RequestId == id);
            if (req == null || req.Status != "Pending")
                return RedirectToPage();

            req.Status = "Rejected";
            req.ProcessedDate = DateTime.UtcNow;
            req.ProcessedByUserId = GetCurrentUserId();
            req.AdminComments = RejectionReason;
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }
    }
} 
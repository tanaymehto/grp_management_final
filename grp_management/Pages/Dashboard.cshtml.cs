using Microsoft.AspNetCore.Mvc.RazorPages;
using grp_management.Data;
using grp_management.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace grp_management.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<object> RecentMessages { get; set; } = new List<object>();
        public Dictionary<string, int> MessagesBySentViaChartData { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MonthlyMessageCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> GroupApprovalChartData { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> DailyMessageCounts { get; set; } = new Dictionary<string, int>();

        public DashboardModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var username = User.Identity.Name;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            IQueryable<SentMsg> query;

            if (user != null && user.Role == "Admin")
            {
                // Admins see all messages
                query = _context.SentMsgs;
            }
            else if (user != null)
            {
                // Regular users only see their own sent messages
                var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (contact != null && contact.EmployeeId.HasValue)
                {
                    var employeeId = contact.EmployeeId.Value;
                    query = _context.SentMsgs.Where(m => m.SenderEmployeeId == employeeId);
                }
                else
                {
                    // User not linked to an employee, show no messages
                    query = _context.SentMsgs.Where(m => false);
                }
            }
            else
            {
                // Not logged in, show no messages
                query = _context.SentMsgs.Where(m => false);
            }

            // 1. Recent Messages (now correctly filtered by role)
            RecentMessages = await query
                .Include(s => s.Group)
                .Include(s => s.Sender)
                .OrderByDescending(m => m.SentDate)
                .Take(4)
                .Select(s => new 
                {
                    SentDate = s.SentDate,
                    GroupName = s.Group != null ? s.Group.Name : "N/A",
                    SenderName = s.Sender != null ? s.Sender.Name : "N/A",
                    SentVia = s.SentVia ?? "N/A",
                    MessageContent = s.MessageContent,
                    Status = s.Status ?? "Unknown"
                })
                .Cast<object>()
                .ToListAsync();

            // 2. Monthly Message Count (last 30 days)
            MonthlyMessageCounts = await _context.SentMsgs
                .Where(m => m.SentDate >= DateTime.UtcNow.AddDays(-30))
                .GroupBy(m => m.SentDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToDictionaryAsync(k => k.Date.ToString("MMM dd"), v => v.Count);

            // 3. Messages by Sent Via
            MessagesBySentViaChartData = await _context.SentMsgs
                .Where(m => m.SentVia != null)
                .GroupBy(m => m.SentVia)
                .Select(g => new { SentVia = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.SentVia!, v => v.Count);
            
            // 4. Group Approval Status
            var approvedCount = await _context.GroupMembershipRequests.CountAsync(r => r.Status == "Approved");
            var pendingCount = await _context.GroupMembershipRequests.CountAsync(r => r.Status == "Pending");
            var rejectedCount = await _context.GroupMembershipRequests.CountAsync(r => r.Status == "Rejected");
            GroupApprovalChartData = new Dictionary<string, int>
            {
                { "Approved", approvedCount },
                { "Pending", pendingCount },
                { "Rejected", rejectedCount }
            };

            // 5. Daily Message Count (last 7 days)
            DailyMessageCounts = await _context.SentMsgs
                .Where(m => m.SentDate >= DateTime.UtcNow.AddDays(-7))
                .GroupBy(m => m.SentDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToDictionaryAsync(k => k.Date.ToString("ddd"), v => v.Count);
        }
    }
} 
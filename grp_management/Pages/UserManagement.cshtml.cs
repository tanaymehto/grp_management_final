using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using grp_management.Data;
using grp_management.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace grp_management.Pages
{
    [Authorize(Roles = "Admin")]
    public class UserManagementModel : PageModel
    {
        private readonly AppDbContext _context;
        public UserManagementModel(AppDbContext context) { _context = context; }

        public List<User> Users { get; set; } = new();
        public Dictionary<int, int> UserGroupCounts { get; set; } = new();
        public User? SelectedUser { get; set; }
        public List<Group> SelectedUserGroups { get; set; } = new();
        public List<Group> AvailableGroupsToAdd { get; set; } = new();

        [BindProperty]
        public int userId { get; set; }
        [BindProperty]
        public int groupId { get; set; }

        public async Task OnGetAsync()
        {
            await LoadUsersAndCounts();
        }

        public async Task<IActionResult> OnPostSelectUserAsync(int userId)
        {
            await LoadUsersAndCounts();
            SelectedUser = await _context.Users.FindAsync(userId);
            if (SelectedUser != null)
            {
                var userGroups = await _context.Contacts
                    .Where(c => c.UserId == userId)
                    .Include(c => c.Group)
                    .Select(c => c.Group!)
                    .ToListAsync();
                SelectedUserGroups = userGroups;
                var allGroups = await _context.Groups.ToListAsync();
                AvailableGroupsToAdd = allGroups.Where(g => !userGroups.Any(ug => ug.Id == g.Id)).ToList();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToGroupAsync(int userId, int groupId)
        {
            // Add to Contacts (existing logic)
            var contact = new Contact { UserId = userId, GroupId = groupId, UserName = (await _context.Users.FindAsync(userId))?.Username ?? "", AccessLevel = "Member" };
            _context.Contacts.Add(contact);

            // Also add to GroupEmployees if not already present
            var employeeId = await _context.Contacts
                .Where(c => c.UserId == userId && c.EmployeeId.HasValue)
                .Select(c => c.EmployeeId.Value)
                .FirstOrDefaultAsync();
            if (employeeId != 0)
            {
                bool alreadyInGroup = await _context.GroupEmployees.AnyAsync(ge => ge.EmployeeId == employeeId && ge.GroupId == groupId);
                if (!alreadyInGroup)
                {
                    var groupEmployee = new GroupEmployee { GroupId = groupId, EmployeeId = employeeId };
                    _context.GroupEmployees.Add(groupEmployee);
                }
            }
            await _context.SaveChangesAsync();
            return await OnPostSelectUserAsync(userId);
        }

        public async Task<IActionResult> OnPostRemoveFromGroupAsync(int userId, int groupId)
        {
            // Remove from Contacts (existing logic)
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == userId && c.GroupId == groupId);
            int? employeeId = contact?.EmployeeId;
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            // Also remove from GroupEmployees if present
            if (employeeId.HasValue)
            {
                var groupEmployee = await _context.GroupEmployees.FirstOrDefaultAsync(ge => ge.EmployeeId == employeeId.Value && ge.GroupId == groupId);
                if (groupEmployee != null)
                {
                    _context.GroupEmployees.Remove(groupEmployee);
                }
            }
            await _context.SaveChangesAsync();
            return await OnPostSelectUserAsync(userId);
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                // Remove all contacts for this user
                var contacts = _context.Contacts.Where(c => c.UserId == id);
                _context.Contacts.RemoveRange(contacts);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            await LoadUsersAndCounts();
            return Page();
        }

        private async Task LoadUsersAndCounts()
        {
            Users = await _context.Users.ToListAsync();
            var groupCounts = await _context.Contacts
                .GroupBy(c => c.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();
            UserGroupCounts = Users.ToDictionary(u => u.Id, u => groupCounts.FirstOrDefault(gc => gc.UserId == u.Id)?.Count ?? 0);
        }
    }
}
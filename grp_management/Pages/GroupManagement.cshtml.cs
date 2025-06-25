using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using grp_management.Data;
using grp_management.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace grp_management.Pages
{
    [Authorize]
    public class GroupManagementModel : PageModel
    {
        private readonly AppDbContext _context;

        public GroupManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Group> Groups { get; set; } = new List<Group>();
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public Dictionary<int, int> GroupMemberCounts { get; set; } = new Dictionary<int, int>();
        public Dictionary<int, List<(int EmployeeId, string Name, string Email)>> GroupMembers { get; set; } = new();
        public int? CurrentEmployeeId { get; set; }

        [BindProperty]
        public Group Group { get; set; } = new Group() { Name = string.Empty };

        public async Task OnGetAsync()
        {
            Groups = await _context.Groups.ToListAsync();
            Employees = await _context.Employees.ToListAsync();
            GroupMemberCounts = await _context.Groups
                .Select(g => new { g.Id, Count = _context.GroupEmployees.Count(ge => ge.GroupId == g.Id) })
                .ToDictionaryAsync(x => x.Id, x => x.Count);

            // Get current user's EmployeeId (if any)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            if (user != null)
            {
                var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (contact != null && contact.EmployeeId.HasValue)
                    CurrentEmployeeId = contact.EmployeeId.Value;
            }

            // For each group, get the list of members (EmployeeId, Name, Email)
            GroupMembers = await _context.Groups
                .Select(g => new {
                    g.Id,
                    Members = _context.GroupEmployees
                        .Where(ge => ge.GroupId == g.Id)
                        .Select(ge => new { ge.EmployeeId, ge.Employee.Name, ge.Employee.Email })
                        .ToList()
                })
                .ToDictionaryAsync(
                    x => x.Id,
                    x => x.Members.Select(m => (m.EmployeeId, m.Name, m.Email)).ToList()
                );
        }

        public async Task<IActionResult> OnPostAddGroupAsync()
        {
            if (!ModelState.IsValid)
            {
                Groups = await _context.Groups.ToListAsync();
                Employees = await _context.Employees.ToListAsync();
                return Page();
            }

            _context.Groups.Add(Group);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Group added successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditGroupAsync()
        {
            if (!ModelState.IsValid)
            {
                Groups = await _context.Groups.ToListAsync();
                Employees = await _context.Employees.ToListAsync();
                return Page();
            }

            _context.Attach(Group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Message"] = "Group updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Groups.AnyAsync(e => e.Id == Group.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteGroupAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Group deleted successfully!";
            }
            return RedirectToPage();
        }
    }
} 
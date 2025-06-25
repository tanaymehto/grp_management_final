using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using grp_management.Data;
using grp_management.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grp_management.Pages
{
    [Authorize]
    public class GroupEmployeesModel : PageModel
    {
        private readonly AppDbContext _context;

        public GroupEmployeesModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int SelectedGroupId { get; set; }
        [BindProperty]
        public int SelectedEmployeeId { get; set; }

        public SelectList Groups { get; set; } = null!;
        public SelectList EmployeesDropdown { get; set; } = null!;

        public List<UserGroupDisplayModel> MyGroups { get; set; } = new();
        public List<Group> AvailableGroupsToJoin { get; set; } = new();

        [BindProperty]
        public int groupId { get; set; }

        public class UserGroupDisplayModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string? Reason { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var employeeIdClaim = User.FindFirstValue("EmployeeId");
            if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out var employeeId))
            {
                // This will trigger a challenge and redirect to login if the claim is missing
                return Challenge();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmpNO == employeeId);
            if (employee == null)
            {
                // This case should ideally not happen if claims are set correctly upon login
                return NotFound($"Employee profile not found for user with EmployeeId: {employeeId}");
            }

            // 1. Get all approved group memberships from GroupEmployees
            var approvedGroupIds = await _context.GroupEmployees
                .Where(ge => ge.EmployeeId == employeeId)
                .Select(ge => ge.GroupId)
                .ToListAsync();

            var approvedGroups = await _context.Groups
                .Where(g => approvedGroupIds.Contains(g.Id))
                .Select(g => new UserGroupDisplayModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Status = "Approved",
                    Reason = ""
                }).ToListAsync();

            // 2. Get the latest request for all other groups the user has interacted with
            var latestRequests = await _context.GroupMembershipRequests
                .Where(gmr => gmr.EmployeeId == employeeId && !approvedGroupIds.Contains(gmr.GroupId))
                .GroupBy(gmr => gmr.GroupId)
                .Select(g => g.OrderByDescending(r => r.RequestDate)
                               .Select(r => new UserGroupDisplayModel {
                                    Id = r.GroupId,
                                    Name = r.Group.Name,
                                    Description = r.Group.Description,
                                    Status = r.Status,
                                    Reason = r.AdminComments
                               })
                               .First())
                .ToListAsync();

            // 3. Combine the lists and order
            MyGroups = approvedGroups.Concat(latestRequests)
                                     .OrderBy(g => g.Name)
                                     .ToList();

            // 4. Get all groups the user can join
            var myGroupIds = MyGroups.Select(g => g.Id).ToList();
            AvailableGroupsToJoin = await _context.Groups
                .Where(g => !myGroupIds.Contains(g.Id))
                .OrderBy(g => g.Name)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddEmployeeToGroupAsync()
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
                EmployeesDropdown = new SelectList(await _context.Employees.ToListAsync(), "EmpNO", "Name");
                return Page();
            }

            // Check if the employee is already in the group
            var existingGroupEmployee = await _context.GroupEmployees
                .AnyAsync(ge => ge.GroupId == SelectedGroupId && ge.EmployeeId == SelectedEmployeeId);

            if (existingGroupEmployee)
            {
                ModelState.AddModelError(string.Empty, "Employee is already a member of this group.");
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
                EmployeesDropdown = new SelectList(await _context.Employees.ToListAsync(), "EmpNO", "Name");
                return Page();
            }

            // Retrieve the actual Group and Employee entities
            var group = await _context.Groups.FindAsync(SelectedGroupId);
            var employee = await _context.Employees.FindAsync(SelectedEmployeeId);

            if (group == null || employee == null)
            {
                ModelState.AddModelError(string.Empty, "Selected group or employee not found.");
                Groups = new SelectList(await _context.Groups.ToListAsync(), "Id", "Name");
                EmployeesDropdown = new SelectList(await _context.Employees.ToListAsync(), "EmpNO", "Name");
                return Page();
            }

            var groupEmployee = new GroupEmployee
            {
                GroupId = SelectedGroupId,
                EmployeeId = SelectedEmployeeId
            };

            _context.GroupEmployees.Add(groupEmployee);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Employee added to group successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRequestAccessAsync(int groupId)
        {
            var employeeIdClaim = User.FindFirstValue("EmployeeId");
            if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out var employeeId))
            {
                return Challenge();
            }

            // Check if already requested or member
            bool alreadyRequested = await _context.GroupMembershipRequests.AnyAsync(r => r.EmployeeId == employeeId && r.GroupId == groupId);
            bool alreadyMember = await _context.GroupEmployees.AnyAsync(ge => ge.EmployeeId == employeeId && ge.GroupId == groupId);
            if (!alreadyRequested && !alreadyMember)
            {
                var req = new GroupMembershipRequest
                {
                    GroupId = groupId,
                    EmployeeId = employeeId,
                    RequestDate = DateTime.UtcNow,
                    Status = "Pending"
                };
                _context.GroupMembershipRequests.Add(req);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
} 
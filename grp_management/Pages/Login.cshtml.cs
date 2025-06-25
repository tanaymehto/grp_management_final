using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using grp_management.Models;
using grp_management.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace grp_management.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(AppDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        [Required]
        public required string Username { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return LocalRedirect(returnUrl ?? "/Dashboard");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);

            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            _logger.LogInformation("Attempting to verify password for user: {Username}", Username);
            _logger.LogInformation("Entered password: {EnteredPassword}", Password);
            _logger.LogInformation("Stored password hash: {StoredPasswordHash}", user.PasswordHash);

            // Compare the entered password with the stored hashed password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                ErrorMessage = "Invalid username or password.";
                _logger.LogWarning("Password verification failed for user: {Username}", Username);
                return Page();
            }

            _logger.LogInformation("Password verification successful for user: {Username}", Username);

            // Create authentication cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Find the associated contact and employee for the user
            var contactWithEmployee = await _context.Contacts
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (contactWithEmployee?.Employee != null)
            {
                claims.Add(new Claim(ClaimTypes.Email, contactWithEmployee.Employee.Email));
                claims.Add(new Claim("EmployeeId", contactWithEmployee.Employee.EmpNO.ToString()));
            }
            else
            {
                // Always set EmployeeId claim for regular users to avoid redirect loop
                if (user.Role == "User")
                {
                    claims.Add(new Claim("EmployeeId", "-1"));
                }
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Keep the user logged in across browser sessions
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // Cookie expiration time
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            returnUrl ??= Url.Content("~/Dashboard");
            return LocalRedirect(returnUrl);
        }
    }
}
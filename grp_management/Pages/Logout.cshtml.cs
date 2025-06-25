using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace grp_management.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            // Clear session
            HttpContext.Session.Clear();
            
            // Delete authentication cookie
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            
            // Redirect to login page
            return RedirectToPage("/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Do NOT call HttpContext.Session.Clear() if session is not configured
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToPage("/Login");
        }
    }
}
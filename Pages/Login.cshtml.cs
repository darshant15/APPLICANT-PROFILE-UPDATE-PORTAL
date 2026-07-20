using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KHBAddressPortal.Pages;

public class LoginModel : PageModel
{
    [BindProperty, Required]
    public string LoginType { get; set; } = "Applicant";

    [BindProperty, Required(ErrorMessage = "Application number or staff ID is required.")]
    public string UserId { get; set; } = string.Empty;

    [BindProperty, Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        HttpContext.Session.Clear();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var isApplicant = LoginType == "Applicant"
                          && UserId.Equals("APP202600123", StringComparison.OrdinalIgnoreCase)
                          && Password == "Applicant@123";
        var isAdmin = LoginType == "Admin"
                      && UserId.Equals("REG-014", StringComparison.OrdinalIgnoreCase)
                      && Password == "Admin@123";
        if (!isApplicant && !isAdmin)
        {
            ErrorMessage = "Invalid ID or password.";
            return Page();
        }

        HttpContext.Session.SetString("UserId", UserId.Trim());
        HttpContext.Session.SetString("UserRole", isAdmin ? "Admin" : "Applicant");
        HttpContext.Session.SetString("OtpPending", "true");
        return RedirectToPage("/VerifyOtp");
    }
}

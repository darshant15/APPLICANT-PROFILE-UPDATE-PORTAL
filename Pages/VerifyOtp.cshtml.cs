using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KHBAddressPortal.Pages;

public class VerifyOtpModel : PageModel
{
    [BindProperty, Required, RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit OTP.")]
    public string Otp { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
    public string? StatusMessage { get; set; }

    public IActionResult OnGet()
    {
        return HasPendingLogin() ? Page() : RedirectToPage("/Login");
    }

    public IActionResult OnPost()
    {
        if (!HasPendingLogin())
            return RedirectToPage("/Login");

        if (!ModelState.IsValid)
            return Page();

        // Demo OTP. Replace with an OTP service and expiry validation in production.
        if (Otp != "123456")
        {
            ErrorMessage = "Incorrect OTP. Please try again.";
            return Page();
        }

        HttpContext.Session.Remove("OtpPending");
        HttpContext.Session.SetString("OtpVerified", "true");
        return HttpContext.Session.GetString("UserRole") == "Admin"
            ? RedirectToPage("/Admin")
            : RedirectToPage("/ApplicantDashboard");
    }

    public IActionResult OnPostResend()
    {
        if (!HasPendingLogin())
            return RedirectToPage("/Login");

        ModelState.Clear();
        StatusMessage = "A new OTP has been sent successfully.";
        return Page();
    }

    private bool HasPendingLogin() =>
        !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")) &&
        HttpContext.Session.GetString("OtpPending") == "true";
}

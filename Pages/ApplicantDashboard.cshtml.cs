using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KHBAddressPortal.Pages;

public class ApplicantDashboardModel : PageModel
{
    public string ApplicationNumber { get; private set; } = "APP202600123";
    public string? LastRequestId { get; private set; }
    public string? LastRequestType { get; private set; }

    public IActionResult OnGet()
    {
        if (!IsApplicant()) return RedirectToPage("/Login");
        ApplicationNumber = HttpContext.Session.GetString("UserId") ?? "APP202600123";
        LastRequestId = HttpContext.Session.GetString("LastRequestId");
        LastRequestType = HttpContext.Session.GetString("LastRequestType");
        return Page();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Login");
    }

    private bool IsApplicant() => HttpContext.Session.GetString("OtpVerified") == "true"
                                  && HttpContext.Session.GetString("UserRole") == "Applicant";
}

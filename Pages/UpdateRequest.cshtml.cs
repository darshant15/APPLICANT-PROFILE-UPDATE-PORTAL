using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KHBAddressPortal.Pages;

public class UpdateRequestModel : PageModel
{
    [BindProperty(SupportsGet = true)] public string Type { get; set; } = "Address";
    [BindProperty, Required] public string NewValue { get; set; } = string.Empty;
    [BindProperty] public string? AddressLine { get; set; }
    [BindProperty] public string? Area { get; set; }
    [BindProperty] public string? City { get; set; }
    [BindProperty] public string? State { get; set; }
    [BindProperty] public string? PinCode { get; set; }
    [BindProperty, Required] public string Reason { get; set; } = string.Empty;
    [BindProperty, Required] public IFormFile? Document { get; set; }
    [BindProperty, Range(typeof(bool), "true", "true", ErrorMessage = "Please confirm the declaration.")] public bool Confirmed { get; set; }
    public string ApplicationNumber { get; private set; } = "APP202600123";
    public string CurrentValue => Type switch { "Phone" => "9876543210", "Email" => "rahul@gmail.com", "Name" => "Rahul Kumar", _ => "12, MG Road, Ashok Nagar, Bengaluru – 560001" };

    public IActionResult OnGet()
    {
        if (!IsApplicant()) return RedirectToPage("/Login");
        NormalizeType(); LoadApplicant(); return Page();
    }

    public IActionResult OnPost()
    {
        if (!IsApplicant()) return RedirectToPage("/Login");
        NormalizeType(); LoadApplicant();
        if (Type == "Address")
        {
            ModelState.Remove(nameof(NewValue));
            if (string.IsNullOrWhiteSpace(AddressLine) || string.IsNullOrWhiteSpace(City) || !System.Text.RegularExpressions.Regex.IsMatch(PinCode ?? "", @"^\d{6}$"))
                ModelState.AddModelError(string.Empty, "Complete the address and enter a valid 6-digit PIN code.");
        }
        if (Document is { Length: > 5 * 1024 * 1024 }) ModelState.AddModelError(nameof(Document), "Maximum file size is 5 MB.");
        var ext = Path.GetExtension(Document?.FileName ?? "").ToLowerInvariant();
        if (Document != null && ext is not (".pdf" or ".jpg" or ".jpeg" or ".png")) ModelState.AddModelError(nameof(Document), "Only PDF, JPG and PNG files are allowed.");
        if (!ModelState.IsValid) return Page();
        HttpContext.Session.SetString("LastRequestId", $"{Type[..1].ToUpper()}UR-2026-001");
        HttpContext.Session.SetString("LastRequestType", Type);
        return RedirectToPage("/ApplicantDashboard");
    }

    private void NormalizeType(){ if (Type is not ("Address" or "Phone" or "Email" or "Name")) Type = "Address"; }
    private void LoadApplicant()=>ApplicationNumber=HttpContext.Session.GetString("UserId")??"APP202600123";
    private bool IsApplicant()=>HttpContext.Session.GetString("OtpVerified")=="true"&&HttpContext.Session.GetString("UserRole")=="Applicant";
}

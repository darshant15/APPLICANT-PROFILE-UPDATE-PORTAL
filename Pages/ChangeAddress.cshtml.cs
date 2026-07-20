using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KHBAddressPortal.Pages;

public class ChangeAddressModel : PageModel
{
    public string ApplicationNumber { get; private set; } = "APP202600123";
    public string[] DocumentTypes { get; } = ["Aadhaar Card", "Passport", "Driving License", "Rental Agreement"];

    [BindProperty, Required, Display(Name = "House No / Flat No")] public string HouseNo { get; set; } = string.Empty;
    [BindProperty, Required] public string Street { get; set; } = string.Empty;
    [BindProperty, Required, Display(Name = "Area / Locality")] public string Area { get; set; } = string.Empty;
    [BindProperty, Required] public string City { get; set; } = string.Empty;
    [BindProperty, Required] public string State { get; set; } = string.Empty;
    [BindProperty, Required, RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit PIN code."), Display(Name = "PIN Code")] public string PinCode { get; set; } = string.Empty;
    [BindProperty, Required, Display(Name = "Describe briefly")] public string Reason { get; set; } = string.Empty;
    [BindProperty, Required] public string DocumentType { get; set; } = string.Empty;
    [BindProperty, Required, Display(Name = "Upload Document")] public IFormFile? Document { get; set; }
    [BindProperty, Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm the declaration.")] public bool Confirmed { get; set; }
    public string? SuccessMessage { get; set; }

    public IActionResult OnGet() => IsVerified() ? LoadAndShow() : RedirectToPage("/Login");

    public IActionResult OnPost()
    {
        if (!IsVerified()) return RedirectToPage("/Login");
        LoadApplicationNumber();

        if (Document is { Length: > 5 * 1024 * 1024 })
            ModelState.AddModelError(nameof(Document), "The document must not exceed 5 MB.");

        var extension = Path.GetExtension(Document?.FileName ?? "").ToLowerInvariant();
        if (Document != null && extension is not (".pdf" or ".jpg" or ".jpeg" or ".png"))
            ModelState.AddModelError(nameof(Document), "Only PDF, JPG and PNG files are allowed.");

        if (!ModelState.IsValid) return Page();

        // Save the request and document through a service/repository in production.
        SuccessMessage = "Your address change request was submitted successfully.";
        ModelState.Clear();
        return Page();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Login");
    }

    private bool IsVerified() => HttpContext.Session.GetString("OtpVerified") == "true";
    private IActionResult LoadAndShow() { LoadApplicationNumber(); return Page(); }
    private void LoadApplicationNumber()
    {
        var id = HttpContext.Session.GetString("UserId");
        ApplicationNumber = !string.IsNullOrWhiteSpace(id) && id.StartsWith("APP", StringComparison.OrdinalIgnoreCase) ? id : "APP202600123";
    }
}

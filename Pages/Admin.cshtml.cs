using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace KHBAddressPortal.Pages;
public record UpdateRequestRow(string Id,string ApplicationNumber,string Applicant,string Type,string CurrentValue,string NewValue,string Document,string Submitted);
public class AdminModel : PageModel
{
    public string StaffId { get; private set; }="REG-014";
    public string? StatusMessage { get; private set; }
    public IReadOnlyList<UpdateRequestRow> Requests { get; }=[
      new("AUR-2026-001","APP202600123","Rahul Kumar","Address","12, MG Road, Bengaluru – 560001","45, Whitefield Main Road, Bengaluru – 560066","Aadhaar Card","20 Jul 2026"),
      new("PUR-2026-002","APP202600123","Rahul Kumar","Phone","9876543210","9123456780","Aadhaar Card","20 Jul 2026"),
      new("EUR-2026-003","APP202600123","Rahul Kumar","Email","rahul@gmail.com","rahul.kumar@email.com","Identity Proof","20 Jul 2026"),
      new("NUR-2026-004","APP202600123","Rahul Kumar","Name","Rahul Kumar","Rahul K. Kumar","Passport","20 Jul 2026")];
    public IActionResult OnGet()=>IsAdmin()?LoadPage():RedirectToPage("/Login");
    public IActionResult OnPostApprove(string requestId)=>Handle($"Request {requestId} was approved successfully.");
    public IActionResult OnPostReject(string requestId)=>Handle($"Request {requestId} was rejected.");
    public IActionResult OnPostLogout(){HttpContext.Session.Clear();return RedirectToPage("/Login");}
    private IActionResult Handle(string message){if(!IsAdmin())return RedirectToPage("/Login");LoadStaff();StatusMessage=message;return Page();}
    private IActionResult LoadPage(){LoadStaff();return Page();}
    private void LoadStaff()=>StaffId=HttpContext.Session.GetString("UserId")??"REG-014";
    private bool IsAdmin()=>HttpContext.Session.GetString("OtpVerified")=="true"&&HttpContext.Session.GetString("UserRole")=="Admin";
}

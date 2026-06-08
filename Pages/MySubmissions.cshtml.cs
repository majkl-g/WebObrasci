using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages
{
    [Authorize]
    public class MySubmissionsModel : PageModel
    {
        private readonly AppDbContext _context;
        public MySubmissionsModel(AppDbContext context) => _context = context;

        public List<FormSubmission> Submissions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var externalId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(externalId))
                return;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user == null)
                return;

            Submissions = await _context.FormSubmissions
                .Include(s => s.Form)
                .Include(s => s.Approvals)
                .Where(s => s.UserId == user.Id)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();
        }
    }
}
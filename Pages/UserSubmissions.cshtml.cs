using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages
{
    [Authorize]
    public class UserSubmissionsModel : PageModel
    {
        private readonly AppDbContext _context;
        public UserSubmissionsModel(AppDbContext context) => _context = context;

        public List<FormSubmission> Submissions { get; set; } = new();

        public async Task OnGetAsync()
        {
            Submissions = await _context.FormSubmissions
                .Include(s => s.Form)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();
        }
    }
}
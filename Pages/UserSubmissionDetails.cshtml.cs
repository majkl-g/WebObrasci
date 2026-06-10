using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages
{
    [Authorize]
    public class UserSubmissionDetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        public UserSubmissionDetailsModel(AppDbContext context) => _context = context;

        public FormSubmission? Submission { get; set; }
        public List<(string Question, string Answer)> Qa { get; set; } = new();

        

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["ShowBanner"] = false;

            Submission = await _context.FormSubmissions
                .Include(s => s.Form)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Submission == null)
                return NotFound();

            Qa = ParseSimpleJson(Submission.DataJson);

            return Page();
        }

        private List<(string Question, string Answer)> ParseSimpleJson(string json)
        {
            
            if (string.IsNullOrWhiteSpace(json))
                return new();

            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (dict == null) return new();
                return dict.Select(kv => (kv.Key, kv.Value)).ToList();
            }
            catch
            {
                return new();
            }
        }
    }
}
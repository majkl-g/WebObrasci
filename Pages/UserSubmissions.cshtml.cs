using System.Security.Claims;
using System.Text.Json;
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
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();
        }

        public List<(string Question, string Answer)> ParseSimpleJson(string json)
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
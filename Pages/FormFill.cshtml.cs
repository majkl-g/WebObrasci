using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = "Student")]
    public class FormFillModel : PageModel
    {
        private readonly AppDbContext _context;
        public FormFillModel(AppDbContext context) => _context = context;

        public DynamicForm Form { get; set; } = null!;

        [BindProperty]
        public Dictionary<string, string> Answers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Form = await _context.DynamicForms
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (Form == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Form = await _context.DynamicForms
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (Form == null) return NotFound();

            // Manual validation for required fields
            foreach (var field in Form.Fields.Where(f => f.Required))
            {
                if (!Answers.TryGetValue(field.Name, out var value) || string.IsNullOrWhiteSpace(value))
                {
                    ModelState.AddModelError($"Answers[{field.Name}]", $"{field.Label} is required.");
                }
            }

            if (!ModelState.IsValid) return Page();

            var externalId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(externalId)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);
            if (user == null) return Unauthorized();

            var submission = new DynamicFormSubmission
            {
                FormId = Form.Id,
                UserId = user.Id,
                DataJson = JsonSerializer.Serialize(Answers),
                SubmittedAt = DateTime.UtcNow
            };

            _context.DynamicFormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Privacy");
        }
    }
}

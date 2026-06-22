using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebObrasci1.Data;
using WebObrasci1.Dto;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Student)]
    public class FormFillModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;

        public FormFillModel(AppDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public DtoForm Form { get; set; } = null!;

        [BindProperty]
        public Dictionary<string, string> Answers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["ShowBanner"] = false;
            var form = await _context.Forms
                .Where(x => x.Enabled)
                .Include(f => f.Fields)
                .ThenInclude(x => x.SelectValues)
                .Include(f => f.Fields)
                .ThenInclude(x => x.FormAutofillMapping)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null) return NotFound();

            Form = DtoForm.FromForm(form);

            foreach (var field in Form.Fields)
            {
                if (field.Field.FormAutofillMapping != null)
                {
                    field.AutofillValue = _userHelper.GetValue(User, field.Field.FormAutofillMapping.Mapping);
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var form = await _context.Forms
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null) return NotFound();

            Form = DtoForm.FromForm(form);

            // Manual validation for required fields
            foreach (var field in form.Fields.Where(f => f.Required))
            {
                if (!Answers.TryGetValue(field.Name, out var value) || string.IsNullOrWhiteSpace(value))
                {
                    ModelState.AddModelError($"Answers[{field.Name}]", $"{field.Label} is required.");
                }
            }

            if (!ModelState.IsValid) return Page();

            var user = await _userHelper.GetOrCreateUserAsync(User);

            var submission = new FormSubmission
            {
                FormId = form.Id,
                UserId = user.Id,
                DataJson = JsonSerializer.Serialize(Answers),
                SubmittedAt = DateTime.UtcNow
            };

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/FormsList");
        }

    }
}

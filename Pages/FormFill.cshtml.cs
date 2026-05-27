using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebObrasci1.Data;
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

        public Form Form { get; set; } = null!;

        [BindProperty]
        public Dictionary<string, string> Answers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var form = await _context.Forms
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null) return NotFound();
            Form = form;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var form = await _context.Forms
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null) return NotFound();

            Form = form;

            // Manual validation for required fields
            foreach (var field in Form.Fields.Where(f => f.Required))
            {
                if (!Answers.TryGetValue(field.Name, out var value) || string.IsNullOrWhiteSpace(value))
                {
                    ModelState.AddModelError($"Answers[{field.Name}]", $"{field.Label} is required.");
                }
            }

            if (!ModelState.IsValid) return Page();

            var user = await GetOrCreateUserAsync();

            var submission = new FormSubmission
            {
                FormId = Form.Id,
                UserId = user.Id,
                DataJson = JsonSerializer.Serialize(Answers),
                SubmittedAt = DateTime.UtcNow
            };

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Privacy");
        }

        private async Task<User> GetOrCreateUserAsync()
        {
            var externalId = _userHelper.GetUserId(User);

            // Keycloak username
            var username = User
                .FindFirst("preferred_username")?.Value;

            // Keycloak email
            var email = User
                .FindFirst("email")?.Value;

            // Check if user already exists
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.ExternalId == externalId);

            if (user == null)
            {
                user = new User
                {
                    ExternalId = externalId,
                    UserName = username ?? "",
                    Email = email ?? ""
                };

                _context.Users.Add(user);
            }
            else
            {
                // Update user info on login
                user.UserName = username ?? user.UserName;
                user.Email = email ?? user.Email;
            }

            await _context.SaveChangesAsync();
            return user;
        }
    }
}

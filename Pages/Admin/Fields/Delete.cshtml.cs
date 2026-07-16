using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Fields
{
    [Authorize(Roles = Role.Profesor)]
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public FormField Field { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["ShowBanner"] = false;

            var field = await _context.FormFields.FindAsync(id);
            if (field == null) return NotFound();
            Field = field;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var field = await _context.FormFields.FindAsync(Field.Id);
            if (field != null)
            {
                _context.FormFields.Remove(field);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("IndexField", new { formId = field?.FormId });
        }
    }
}

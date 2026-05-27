using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Fields
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

        [BindProperty]
        public FormField Field { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var field = await _context.FormFields.FindAsync(id);
            if (field == null) return NotFound();
            Field = field;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.FormFields.Update(Field);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index", new { formId = Field.FormId });
        }
    }
}

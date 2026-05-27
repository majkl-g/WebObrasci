using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Fields
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public FormField Field { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Field = await _context.FormFields.FindAsync(id);
            if (Field == null) return NotFound();
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

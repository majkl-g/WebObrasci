using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
            ViewData["ShowBanner"] = false;

            var field = await _context.FormFields
                .Include(x => x.SelectValues)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (field == null) return NotFound();
            Field = field;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            using var tr = await _context.Database.BeginTransactionAsync();

            await _context.FormFieldSelectValues
                .Where(x => x.FormFieldId == Field.Id)
                .ExecuteDeleteAsync();

            _context.FormFields.Update(Field);
            await _context.SaveChangesAsync();
            
            tr.Commit();

            return RedirectToPage("IndexField", new { formId = Field.FormId });
        }
    }
}

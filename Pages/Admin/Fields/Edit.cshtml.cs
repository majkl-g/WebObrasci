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
        public DynamicFormField Field { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Field = await _context.DynamicFormFields.FindAsync(id);
            if (Field == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.DynamicFormFields.Update(Field);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index", new { formId = Field.DynamicFormId });
        }
    }
}

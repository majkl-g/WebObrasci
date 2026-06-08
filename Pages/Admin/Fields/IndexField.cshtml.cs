using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Fields
{
    public class IndexFieldModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexFieldModel(AppDbContext context) => _context = context;

        public List<FormField> Fields { get; set; } = new();

        [BindProperty]
        public Form Form { get; set; } = null!;

        public int FormId { get; set; }

        public async Task<IActionResult> OnGetAsync(int formId)
        {
            FormId = formId;

            Form = await _context.Forms.FindAsync(formId);
            if (Form == null) return NotFound();

            Fields = await _context.FormFields
                .Where(x => x.FormId == formId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var form = await _context.Forms.FindAsync(Form.Id);
            if (form == null) return NotFound();

            form.Title = Form.Title;

            await _context.SaveChangesAsync();
            return RedirectToPage(new { formId = Form.Id });
        }
    }
}
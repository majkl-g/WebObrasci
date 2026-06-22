using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Pages.Admin.Forms;

namespace WebObrasci1.Pages.Admin.Fields
{
    public class IndexFieldModel : FormModel
    {
        private readonly AppDbContext _context;
        public IndexFieldModel(AppDbContext context) => _context = context;

        public List<FormField> Fields { get; set; } = new();

        public int FormId { get; set; }

        public async Task<IActionResult> OnGetAsync(int formId)
        {
            ViewData["ShowBanner"] = false;
            FormId = formId;

            var form = await _context.Forms.FindAsync(formId);
            if (form == null) return NotFound();

            Form = form;
            Fields = await _context.FormFields
                .Where(x => x.FormId == formId)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Forms.Update(Form);
            await _context.SaveChangesAsync();
            return RedirectToPage(new { formId = Form.Id });
        }
    }
}
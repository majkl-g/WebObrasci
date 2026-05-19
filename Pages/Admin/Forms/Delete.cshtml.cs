using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Forms
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        [BindProperty]
        public DynamicForm Form { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Form = await _context.DynamicForms.FindAsync(id);
            if (Form == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var form = await _context.DynamicForms.FindAsync(Form.Id);
            if (form != null)
            {
                _context.DynamicForms.Remove(form);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("IndexForm"); //...
        }
    }
}

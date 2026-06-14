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
        public Form Form { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["ShowBanner"] = false;

            var form = await _context.Forms.FindAsync(id);
            if (form == null) return NotFound();
            Form = form;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var form = await _context.Forms.FindAsync(Form.Id);
            if (form != null)
            {
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("IndexForm"); //...
        }
    }
}

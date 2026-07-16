using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Forms
{
    [Authorize(Roles = Role.Profesor)]
    public class DeleteModel : FormModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

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

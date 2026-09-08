using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Forms
{
    [Authorize(Roles = Role.Profesor)]
    public class EditModel : FormModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

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
            if (!ModelState.IsValid) return Page();

            _context.Update(Form);
            await _context.SaveChangesAsync();
            return RedirectToPage("IndexForm"); //takoðer
        }
    }
}

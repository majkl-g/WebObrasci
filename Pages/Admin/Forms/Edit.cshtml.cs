using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Forms
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

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
            if (!ModelState.IsValid) return Page();

            _context.Update(Form);
            await _context.SaveChangesAsync();
            return RedirectToPage("IndexForm"); //takoðer
        }
    }
}

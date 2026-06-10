using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Forms
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        [BindProperty]
        public Form Form { get; set; }

        public void OnGet() 
        {
            ViewData["ShowBanner"] = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {          
            if (!ModelState.IsValid) return Page();

            _context.Forms.Add(Form);
            await _context.SaveChangesAsync();
            return RedirectToPage("IndexForm");  

        }
    }
}

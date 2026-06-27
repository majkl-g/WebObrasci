using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Forms
{
    [Authorize(Roles = Role.ProfesorOrAdmin)]
    public class CreateModel : FormModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        public void OnGet() 
        {
            Form.Enabled = true;
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

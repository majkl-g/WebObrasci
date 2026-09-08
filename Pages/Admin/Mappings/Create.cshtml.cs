using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Mappings
{
    [Authorize(Roles = Role.Profesor)]
    public class CreateModel : MappingModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        public void OnGet() 
        {
            ViewData["ShowBanner"] = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {          
            if (!ModelState.IsValid) return Page();

            _context.FormAutofillMappings.Add(Mapping);
            await _context.SaveChangesAsync();
            return RedirectToPage("IndexMapping");  

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Mappings
{
    [Authorize(Roles = Role.Profesor)]
    public class DeleteModel : MappingModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["ShowBanner"] = false;

            var mapping = await _context.FormAutofillMappings.FindAsync(id);
            if (mapping == null) return NotFound();
            Mapping = mapping;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var mapping = await _context.FormAutofillMappings.FindAsync(Mapping.Id);
            if (mapping != null)
            {
                _context.FormAutofillMappings.Remove(mapping);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("IndexMapping"); //...
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Mappings
{
    [Authorize(Roles = Role.Profesor)]
    public class EditModel : MappingModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

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
            if (!ModelState.IsValid) return Page();

            _context.Update(Mapping);
            await _context.SaveChangesAsync();
            return RedirectToPage("IndexMapping"); //takoðer
        }
    }
}

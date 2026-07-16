using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Fields
{
    [Authorize(Roles = Role.Profesor)]
    public class EditModel : FieldModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) => _context = context;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["ShowBanner"] = false;

            var field = await _context.FormFields
                .Include(x => x.SelectValues)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (field == null) return NotFound();

            Field = field;
            AvailableMappings = await _context
                .FormAutofillMappings
                .Where(x => x.Active)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            using var tr = await _context.Database.BeginTransactionAsync();

            await _context.FormFieldSelectValues
                .Where(x => x.FormFieldId == Field.Id)
                .ExecuteDeleteAsync();

            _context.FormFields.Update(Field);
            await _context.SaveChangesAsync();
            
            tr.Commit();

            return RedirectToPage("IndexField", new { formId = Field.FormId });
        }
    }
}

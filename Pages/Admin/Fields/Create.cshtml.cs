using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Fields
{
    [Authorize(Roles = Role.Profesor)]
    public class CreateModel : FieldModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        public async Task OnGet(int formId)
        {
            ViewData["ShowBanner"] = false;
            Field.FormId = formId;
            AvailableMappings = await _context
                .FormAutofillMappings
                .Where(x => x.Active)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    Console.WriteLine(error.ErrorMessage);

                return Page();
            }
                       
            // if (!ModelState.IsValid) return Page();

            var currentMax = await _context.FormFields
                .Where(x => x.FormId == Field.FormId)
                .MaxAsync(x => x.Order);

            Field.Order = (currentMax ?? 0) + 1;

            _context.FormFields.Add(Field);
            await _context.SaveChangesAsync();

            return RedirectToPage("IndexField", new { formId = Field.FormId });
        }
    }
}

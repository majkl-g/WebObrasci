using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Fields
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) => _context = context;

        [BindProperty]
        public FormField Field { get; set; } = new();

        public void OnGet(int formId)
        {
            ViewData["ShowBanner"] = false;
            Field.FormId = formId;
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

            _context.FormFields.Add(Field);
            await _context.SaveChangesAsync();

            return RedirectToPage("IndexField", new { formId = Field.FormId });
        }
    }
}

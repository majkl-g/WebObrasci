using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Fields
{
    public class IndexFieldModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexFieldModel(AppDbContext context) => _context = context;

        public List<DynamicFormField> Fields { get; set; }
        public int FormId { get; set; }

        public async Task OnGetAsync(int formId)
        {
            FormId = formId;
            Fields = await _context.DynamicFormFields
                .Where(x => x.DynamicFormId == formId)
                .ToListAsync();
        }
    }
}

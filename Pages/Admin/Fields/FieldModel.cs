using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Fields
{
    public abstract class FieldModel : PageModel
    {
        [BindProperty]
        public FormField Field { get; set; } = new();

        public IList<FormAutofillMapping> AvailableMappings { get; set; } = [];
    }
}

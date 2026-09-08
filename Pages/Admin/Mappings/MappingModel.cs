using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Mappings
{
    public abstract class MappingModel : PageModel
    {
        [BindProperty]
        public FormAutofillMapping Mapping { get; set; } = new();
    }
}

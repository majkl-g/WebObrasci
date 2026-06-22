using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Forms
{
    public abstract class FormModel : PageModel
    {
        [BindProperty]
        public Form Form { get; set; } = new();
    }
}

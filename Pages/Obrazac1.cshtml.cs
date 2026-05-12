using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = "Student")]

    public class Obrazac1Model : PageModel
    {
        public void OnGet()
        {
            //User.Claims
        }
    }
}

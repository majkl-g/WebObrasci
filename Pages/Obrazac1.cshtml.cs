using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Student)]

    public class Obrazac1Model : PageModel
    {
        public void OnGet()
        {
            //User.Claims
        }
    }
}

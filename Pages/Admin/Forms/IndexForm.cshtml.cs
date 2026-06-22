using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Forms
{
    [Authorize(Roles = Role.ProfesorOrAdmin)]
    public class IndexFormModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexFormModel(AppDbContext context) // Fixed constructor name
        {
            _context = context;
            Forms = new List<Form>(); // Initialize Forms to avoid CS8618
        }

        public List<Form> Forms { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["ShowBanner"] = false;
            Forms = await _context.Forms.ToListAsync();
        }
    }
}

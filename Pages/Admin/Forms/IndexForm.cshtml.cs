using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages.Admin.Forms
{
    public class IndexFormModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexFormModel(AppDbContext context) // Fixed constructor name
        {
            _context = context;
            Forms = new List<DynamicForm>(); // Initialize Forms to avoid CS8618
        }

        public List<DynamicForm> Forms { get; set; }

        public async Task OnGetAsync()
        {
            Forms = await _context.DynamicForms.ToListAsync();
        }
    }
}

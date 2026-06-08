using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages
{
    public class FormsListModel : PageModel
    {
        private readonly AppDbContext _context;
        public FormsListModel(AppDbContext context) => _context = context;

        public List<Form> Forms { get; set; } = new();

        public async Task OnGetAsync()
        {
            Forms = await _context.Forms.ToListAsync();
        }
    }
}
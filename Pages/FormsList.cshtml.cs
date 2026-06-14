using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Dto;

namespace WebObrasci1.Pages
{
    public class FormsListModel : PageModel
    {
        private const int _formsPageSize = 5;
        private readonly AppDbContext _context;

        public FormsListModel(AppDbContext context) => _context = context;

        public FormsPage FormsPage { get; set; } = new([], 1, 1, 0);

        public async Task OnGetAsync([FromQuery] int? pageNumber = 1)
        {
            var pageNumChecked = Math.Max(pageNumber ?? 1, 1);
            var skip = _formsPageSize * (pageNumChecked - 1);

            var formsQuery = _context.Forms
                .Where(x => x.Enabled);

            var forms = await formsQuery
                .OrderBy(x => x.Title)
                .Skip(skip)
                .Take(_formsPageSize)
                .ToListAsync();

            var total = await formsQuery
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)total / _formsPageSize);

            FormsPage = new FormsPage(forms, pageNumChecked, _formsPageSize, totalPages);
        }
    }
}
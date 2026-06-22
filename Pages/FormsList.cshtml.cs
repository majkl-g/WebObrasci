using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Pages.Shared;

namespace WebObrasci1.Pages
{
    public class FormsListModel : PagedPageModel<Form>
    {
        private readonly AppDbContext _context;

        protected override bool ShowBanner { get; } = true;

        public FormsListModel(AppDbContext context) => _context = context;

        public override async Task<(IList<Form> Data, int Total)> GetPageDataAsync(int skip, int take)
        {
            var formsQuery = _context.Forms
                .Where(x => x.Enabled);

            var forms = await formsQuery
                .OrderBy(x => x.Title)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var total = await formsQuery
                .CountAsync();

            return (forms,  total);
        }
    }
}
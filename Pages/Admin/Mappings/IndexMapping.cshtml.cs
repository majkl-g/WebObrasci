using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Pages.Shared;
using WebObrasci1.Services;

namespace WebObrasci1.Pages.Admin.Mappings
{
    [Authorize(Roles = Role.Profesor)]
    public class IndexMappingModel : PagedPageModel<FormAutofillMapping>
    {
        private readonly AppDbContext _context;

        public IndexMappingModel(AppDbContext context)
        {
            _context = context;
        }

        public override async Task<(IList<FormAutofillMapping> Data, int Total)> GetPageDataAsync(int skip, int take)
        {
            var data = await _context.FormAutofillMappings.OrderBy(x => x.Id).ToListAsync();
            var total = await _context.FormAutofillMappings.CountAsync();
            return (data, total);
        }
    }
}

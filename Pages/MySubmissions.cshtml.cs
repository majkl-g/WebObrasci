using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Dto;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Student)]
    public class MySubmissionsModel : PageModel
    {
        private const int _submissionPageSize = 5;
        private readonly AppDbContext _context;

        public MySubmissionsModel(AppDbContext context) => _context = context;

        public FormSubmissionsPage SubmissionsPage { get; set; } = new([], 1, 1, 0);

        public async Task OnGetAsync([FromQuery] int? pageNumber = 1)
        {
            var pageNumChecked = Math.Max(pageNumber ?? 1, 1);
            var skip = _submissionPageSize * (pageNumChecked - 1);

            ViewData["ShowBanner"] = false;
            var externalId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(externalId))
                return;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user == null)
                return;

            var submissionsQuery = _context.FormSubmissions
                .Where(s => s.UserId == user.Id);

            var submissions = await submissionsQuery
                .Include(s => s.Form)
                .Include(s => s.Approvals)
                .ThenInclude(x => x.ApprovalUser)
                .OrderByDescending(s => s.SubmittedAt)
                .Skip(skip)
                .Take(_submissionPageSize)
                .ToListAsync();

            var total = await submissionsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)total / _submissionPageSize);

            SubmissionsPage = new FormSubmissionsPage(submissions, pageNumChecked, _submissionPageSize, totalPages);
        }
    }
}
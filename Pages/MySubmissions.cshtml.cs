using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Pages.Shared;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Student)]
    public class MySubmissionsModel : PagedPageModel<FormSubmission>
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;

        public MySubmissionsModel(AppDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public override async Task<(IList<FormSubmission> Data, int Total)> GetPageDataAsync(int skip, int take)
        {
            var externalId = _userHelper.GetUserId(User);

            if (string.IsNullOrWhiteSpace(externalId))
                return ([], 0);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user == null)
                return ([], 0);

            var submissionsQuery = _context.FormSubmissions
                .Where(s => s.UserId == user.Id);

            var submissions = await submissionsQuery
                .Include(s => s.Form)
                .Include(s => s.Approvals)
                .ThenInclude(x => x.ApprovalUser)
                .OrderByDescending(s => s.SubmittedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var total = await submissionsQuery.CountAsync();
            return (submissions, total);
        }
    }
}
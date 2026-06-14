using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Dto;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Profesor)]
    public class UserSubmissionsModel : PageModel
    {
        private const int _submissionPageSize = 5;
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;

        public UserSubmissionsModel(AppDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public FormSubmissionsPage SubmissionsPage { get; set; } = new([], 1, 1, 0);

        public async Task OnGetAsync([FromQuery] int? pageNumber = 1)
        {
            var pageNumChecked = Math.Max(pageNumber ?? 1, 1);
            var skip = _submissionPageSize * (pageNumChecked - 1);

            ViewData["ShowBanner"] = false;

            var submissions = await _context.FormSubmissions
                .Include(s => s.Form)
                .Include(s => s.User)
                .Include(s => s.Approvals)
                .OrderByDescending(s => s.SubmittedAt)
                .Skip(skip)
                .Take(_submissionPageSize)
                .ToListAsync();

            var total = await _context.FormSubmissions.CountAsync();
            var totalPages = (int)Math.Ceiling((double)total / _submissionPageSize);

            SubmissionsPage = new FormSubmissionsPage(submissions, pageNumChecked, _submissionPageSize, totalPages);
        }

        public async Task<IActionResult> OnPostApproveAsync(int submissionId)
        {
            var submission = await _context.FormSubmissions
                .Include(s => s.Approvals)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null)
                return NotFound();

            if (submission.Approvals.Any(a => a.ApprovalAsRole == Role.Profesor))
            {
                TempData["Message"] = "Veæ prihvaæeno";
                return RedirectToPage();
            }

            var user = await _userHelper.GetOrCreateUserAsync(User);

            var approval = new FormSubmissionApproval
            {
                FormSubmissionId = submission.Id,
                ApprovalUser = user,
                ApprovalAsRole = Role.Profesor,
                ApprovedAt = DateTime.UtcNow
            };

            _context.FormSubmissionApproval.Add(approval);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Obrazac prihvaæen";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDenyAsync(int submissionId)
        {
            var submission = await _context.FormSubmissions
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null)
                return NotFound();

            var user = await _userHelper.GetOrCreateUserAsync(User);

            var denial = new FormSubmissionApproval
            {
                FormSubmissionId = submission.Id,
                ApprovalUser = user,
                ApprovalAsRole = Role.Profesor,
                ApprovedAt = DateTime.UtcNow,
                Denied = true,
            };

            _context.FormSubmissionApproval.Add(denial);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Obrazac odbijen";
            return RedirectToPage();
        }
    }
}
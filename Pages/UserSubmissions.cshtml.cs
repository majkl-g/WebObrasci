using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Profesor)]
    public class UserSubmissionsModel : PageModel
    {
        private readonly AppDbContext _context;
        private const string REQUIRED_APPROVAL_ROLE = "Professor";

        public UserSubmissionsModel(AppDbContext context) => _context = context;

        public List<FormSubmission> Submissions { get; set; } = new();

        public async Task OnGetAsync()
        {
            ViewData["ShowBanner"] = false;
            // Show ALL submissions
            Submissions = await _context.FormSubmissions
                .Include(s => s.Form)
                .Include(s => s.User)
                .Include(s => s.Approvals)
                .OrderByDescending(s => s.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int submissionId)
        {
            var submission = await _context.FormSubmissions
                .Include(s => s.Approvals)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null)
                return NotFound();

            if (submission.Approvals.Any(a => a.ApprovalAsRole == REQUIRED_APPROVAL_ROLE))
            {
                TempData["Message"] = "Veæ prihvaæeno";
                return RedirectToPage();
            }

            var approval = new FormSubmissionApproval
            {
                FormSubmissionId = submission.Id,
                ApprovalFrom = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                ApprovalAsRole = REQUIRED_APPROVAL_ROLE,
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

            var denial = new FormSubmissionApproval
            {
                FormSubmissionId = submission.Id,
                ApprovalFrom = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown",
                ApprovalAsRole = "Denied",
                ApprovedAt = DateTime.UtcNow
            };

            _context.FormSubmissionApproval.Add(denial);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Obrazac odbijen";
            return RedirectToPage();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Pages.Shared;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Profesor)]
    public class UserSubmissionsModel : PagedPageModel<FormSubmission>
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IPdfConverter _pdfConverter;

        public UserSubmissionsModel(AppDbContext context, IUserHelper userHelper, IPdfConverter pdfConverter)
        {
            _context = context;
            _userHelper = userHelper;
            _pdfConverter = pdfConverter;
        }

        public override async Task<(IList<FormSubmission> Data, int Total)> GetPageDataAsync(int skip, int take)
        {
            var submissions = await _context.FormSubmissions
                .Include(s => s.Form)
                .Include(s => s.User)
                .Include(s => s.Approvals)
                .OrderByDescending(s => s.SubmittedAt)
                .ThenByDescending(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var total = await _context.FormSubmissions.CountAsync();

            return (submissions, total);
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

        public async Task<ActionResult> OnPostDownloadPdfAsync(int submissionId)
        {
            var submission = await _context.FormSubmissions
                .Include(x => x.Form)
                .ThenInclude(x => x.Fields)
                .ThenInclude(x => x.SelectValues)
                .Include(x => x.User)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null)
                return NotFound();

            var pdf = _pdfConverter.ConvertToPdf(submission);
            var fileName = $"{submission.Form.Title}_{submission.User.UserName}_{submission.SubmittedAt.ToLongDateString()}.pdf";

            var result = new FileStreamResult(pdf, "application/pdf")
            {
                FileDownloadName = fileName,
            };
            return result;
        }
    }
}
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages
{
    [Authorize]
    public class UserSubmissionDetailsModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;

        public UserSubmissionDetailsModel(AppDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public FormSubmission? Submission { get; set; }
        public List<(string Question, string Answer)> Qa { get; set; } = new();
        public string ExternalUserId = "";


        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["ShowBanner"] = false;

            var query = _context.FormSubmissions
                .Include(s => s.Form)
                .Include(s => s.User)
                .Include(s => s.Approvals)
                .ThenInclude(s => s.ApprovalUser)
                .AsQueryable();

            ExternalUserId = _userHelper.GetUserId(User);

            if (User.IsInRole(Role.Profesor) == false && User.IsInRole(Role.Admin) == false)
            {
                query = query.Where(x => x.User.ExternalId == ExternalUserId);
            }

            Submission = await query
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Submission == null)
                return NotFound();

            Qa = ParseSimpleJson(Submission.DataJson);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int submissionId, int approvalId, string action)
        {
            if (User.IsInRole(Role.Profesor) ||  User.IsInRole(Role.Admin))
            {
                var extId = _userHelper.GetUserId(User);

                if (action == "delete")
                {
                    var toRemove = await _context.FormSubmissionApproval
                        .Where(x => x.Id == approvalId)
                        .Where(x => x.ApprovalUser.ExternalId == extId)
                        .FirstOrDefaultAsync();

                    if (toRemove != null)
                    {
                        _context.FormSubmissionApproval.Remove(toRemove);
                        await _context.SaveChangesAsync();
                    }
                }
                else if(action == "approve" || action == "deny")
                {
                    var user = await _userHelper.GetOrCreateUserAsync(User);
                    var approval = new FormSubmissionApproval
                    {
                        FormSubmissionId = submissionId,
                        ApprovalUser = user,
                        ApprovalAsRole = Role.Profesor,
                        ApprovedAt = DateTime.UtcNow,
                        Denied = action == "deny"
                    };
                    _context.FormSubmissionApproval.Add(approval);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToPage("UserSubmissionDetails", new { id = submissionId });
        }

        private List<(string Question, string Answer)> ParseSimpleJson(string json)
        {
            
            if (string.IsNullOrWhiteSpace(json))
                return new();

            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (dict == null) return new();
                return dict.Select(kv => (kv.Key, kv.Value)).ToList();
            }
            catch
            {
                return new();
            }
        }
    }
}
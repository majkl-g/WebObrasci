using System.Security.Claims;
using WebObrasci1.Models;

namespace WebObrasci1.Services
{
    public interface IUserHelper
    {
        string GetEmail(ClaimsPrincipal principal);
        Task<User> GetOrCreateUserAsync(ClaimsPrincipal User);
        string GetUserId(ClaimsPrincipal principal);
        string GetUserName(ClaimsPrincipal principal);
        string GetValue(ClaimsPrincipal principal, string claimName);
        Task SaveMentorAsync(ClaimsPrincipal principal, string? mentorName, string? mentorEmail);
    }
}
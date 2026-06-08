using System.Security.Claims;

namespace WebObrasci1.Services
{
    public interface IUserHelper
    {
        string GetEmail(ClaimsPrincipal principal);
        string GetUserId(ClaimsPrincipal principal);
        string GetUserName(ClaimsPrincipal principal);
    }
}
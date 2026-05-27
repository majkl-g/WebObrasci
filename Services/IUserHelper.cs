using System.Security.Claims;

namespace WebObrasci1.Services
{
    public interface IUserHelper
    {
        string GetUserId(ClaimsPrincipal principal);
    }
}
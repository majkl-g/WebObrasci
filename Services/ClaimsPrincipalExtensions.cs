using System.Security.Claims;

namespace WebObrasci1.Services
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetName(this ClaimsPrincipal principal)
        {
            if (string.IsNullOrEmpty(principal.Identity?.Name) == false)
                return principal.Identity.Name;

            var nameClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)
                ?? principal.Claims.FirstOrDefault(x => x.Type == "name")
                ?? principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName);

            return nameClaim?.Value ?? "";
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebObrasci1.Settings;

namespace WebObrasci1.Services
{
    public class RoleClaimsTransformer : IClaimsTransformation
    {
        private readonly IOptions<UserSettings> _roleSettings;

        public RoleClaimsTransformer(IOptions<UserSettings> roleSettings)
        {
            _roleSettings = roleSettings;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity!;

            if (principal.IsInRole(_roleSettings.Value.StudentRole) && !principal.IsInRole(Role.Student))
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Student));
            else if (principal.IsInRole(_roleSettings.Value.ProfesorRole) && !principal.IsInRole(Role.Profesor))
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Profesor));
            else if (principal.IsInRole(_roleSettings.Value.AdminRole) && !principal.IsInRole(Role.Admin))
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Admin));

            return Task.FromResult(principal);
        }
    }
}
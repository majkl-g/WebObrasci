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

            if (HasClaimValue(identity, _roleSettings.Value.StudentClaim, _roleSettings.Value.StudentClaimValue))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Student));
            }
            else if (HasClaimValue(identity, _roleSettings.Value.ProfesorClaim, _roleSettings.Value.ProfesorClaimValue)
                && HasClaimValue(identity, _roleSettings.Value.EmailClaim, _roleSettings.Value.ProfesorMailWhitelist.ToArray()))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Profesor));
            }

            return Task.FromResult(principal);
        }

        private bool HasClaimValue(ClaimsIdentity identity, string claimName, params string[] targetValues)
        {
            var claims = identity.Claims.Where(c => c.Type == claimName);
            foreach (var claim in claims)
            {
                if (string.IsNullOrEmpty(claim?.Value) == false)
                {
                    if (targetValues.Contains(claim.Value))
                        return true;
                }
            }
            return false;
        }
    }
}
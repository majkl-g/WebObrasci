using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
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

            if (HasJsonClaimValue(identity, _roleSettings.Value.StudentClaim, _roleSettings.Value.StudentClaimValue))
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Student));
            else if (HasJsonClaimValue(identity, _roleSettings.Value.ProfesorClaim, _roleSettings.Value.ProfesorClaimValue))
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Profesor));
            else if (HasJsonClaimValue(identity, _roleSettings.Value.AdminClaim, _roleSettings.Value.AdminClaimValue))
                identity.AddClaim(new Claim(ClaimTypes.Role, Role.Admin));

            return Task.FromResult(principal);
        }

        private bool HasJsonClaimValue(ClaimsIdentity identity, string claimName, string targetValue)
        {
            var claim = identity.Claims.FirstOrDefault(c => c.Type == claimName);
            if (string.IsNullOrEmpty(claim?.Value) == false)
            {
                try
                {
                    var values = JsonSerializer.Deserialize<string[]>(claim.Value);
                    if (values != null && values.Contains(targetValue))
                        return true;
                }
                catch (JsonException) 
                {
                    //skip this claim
                }

                //fallback if not json array:
                if (claim.Value.Equals(targetValue))
                    return true;
            }
            return false;
        }
    }
}
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;
using System.Text.Json;

namespace WebObrasci1
{
    public static class AuthorizationHelper
    {
        public static Task AppendUserInfoToPrincipalAsync(UserInformationReceivedContext context)
        {
            var identity = (ClaimsIdentity?)context.Principal?.Identity;
            if (identity != null)
            {
                var claimsToAdd = context.User.RootElement
                    .EnumerateObject()
                    .SelectMany(x => x.Value.ValueKind switch {
                        JsonValueKind.String => [new Claim(x.Name, x.Value.ToString())],
                        JsonValueKind.Array => x.Value.EnumerateArray().Select(y => new Claim(x.Name, y.ToString())).ToList(),
                        _ => [],
                    })
                    .Where(x => identity.HasClaim(y => y.Type == x.Type) == false)
                    .ToList();
                
                foreach (var claim in claimsToAdd)
                {
                    identity.AddClaim(claim);
                }
            }
            return Task.CompletedTask;
        }
    }
}

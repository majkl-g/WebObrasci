using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebObrasci1.Data;

namespace WebObrasci1.Services
{
    public class DbRoleClaimsTransformer : IClaimsTransformation
    {
        private readonly AppDbContext _db;

        public DbRoleClaimsTransformer(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ClaimsPrincipal> TransformAsync(
            ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity!;

            var externalId = principal
                .FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(externalId))
                return principal;

            var roles = await _db.UserRoles
                .Include(x => x.Role)
                .Where(x => x.User.ExternalId == externalId)
                .Select(x => x.Role.Name)
                .ToListAsync();

            foreach (var role in roles)
            {
                // Prevent duplicate claims
                if (!identity.Claims.Any(c =>
                    c.Type == ClaimTypes.Role &&
                    c.Value == role))
                {
                    identity.AddClaim(
                        new Claim(ClaimTypes.Role, role));
                }
            }

            return principal;
        }
    }
}
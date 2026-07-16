using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Settings;

namespace WebObrasci1.Services
{
    public class UserHelper : IUserHelper
    {
        private IOptions<UserSettings> _userSettings;
        private AppDbContext _context;

        public UserHelper(IOptions<UserSettings> userSettings, AppDbContext context)
        {
            _userSettings = userSettings;
            _context = context;
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            var id = principal.Claims.FirstOrDefault(x => x.Type == _userSettings.Value.IdClaim);
            if (id?.Value == null)
                throw new UnauthorizedAccessException($"User '{principal.Identity?.Name}' does not have an ID claim '{_userSettings.Value.IdClaim}'");
            return id.Value;
        }

        public string GetUserName(ClaimsPrincipal principal)
        {
            var username = principal.Claims.FirstOrDefault(x => x.Type == _userSettings.Value.UsernameClaim);
            if (username?.Value == null)
                throw new UnauthorizedAccessException($"User '{principal.Identity?.Name}' does not have an Username claim '{_userSettings.Value.UsernameClaim}'");
            return username.Value;
        }

        public string GetEmail(ClaimsPrincipal principal)
        {
            var email = principal.Claims.FirstOrDefault(x => x.Type == _userSettings.Value.EmailClaim);
            if (email?.Value == null)
                throw new UnauthorizedAccessException($"User '{principal.Identity?.Name}' does not have an Email claim '{_userSettings.Value.EmailClaim}'");
            return email.Value;
        }

        public string GetValue(ClaimsPrincipal principal, string claimName)
        {
            var claim = principal.Claims.FirstOrDefault(x => x.Type == claimName);
            if (string.IsNullOrEmpty(claim?.Value) == false)
            {
                if (claim.Value.StartsWith("["))
                {
                    try
                    {
                        var values = JsonSerializer.Deserialize<string[]>(claim.Value);
                        if (values != null)
                            return values.FirstOrDefault() ?? string.Empty;
                    }
                    catch (JsonException)
                    {
                        //ignore
                    }
                }

                return claim.Value;
            }
            return string.Empty;
        }

        public async Task<User> GetOrCreateUserAsync(ClaimsPrincipal User)
        {
            //get data from OIDC user
            var externalId = GetUserId(User);
            var username = GetUserName(User);
            var email = GetEmail(User);

            string role = "";
            if (User.IsInRole(Role.Profesor))
                role = Role.Profesor;
            else if (User.IsInRole(Role.Student))
                role = Role.Student;

            // Check if user already exists
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.ExternalId == externalId);

            if (user == null)
            {
                user = new User
                {
                    ExternalId = externalId,
                    UserName = username ?? "",
                    Email = email ?? "",
                    Role = role,
                    Title = null,
                };

                _context.Users.Add(user);
            }
            else
            {
                // Update user info on login
                user.UserName = username ?? user.UserName;
                user.Email = email ?? user.Email;
            }

            await _context.SaveChangesAsync();
            return user;
        }
    }
}

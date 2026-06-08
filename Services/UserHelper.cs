using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebObrasci1.Settings;

namespace WebObrasci1.Services
{
    public class UserHelper : IUserHelper
    {
        private IOptions<UserSettings> _userSettings;

        public UserHelper(IOptions<UserSettings> userSettings)
        {
            _userSettings = userSettings;
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
    }
}

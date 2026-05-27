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
    }
}

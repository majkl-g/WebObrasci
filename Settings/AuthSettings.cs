namespace WebObrasci1.Settings
{
    public class AuthSettings
    {
        public string AuthUrl { get; set; } = "";
        public string ReturnUrl { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public bool RequireHttpsMetadata { get; set; }
        public IList<string> Scopes { get; set; } = [];
    }
}

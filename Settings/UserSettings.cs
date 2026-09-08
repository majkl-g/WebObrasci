namespace WebObrasci1.Settings
{
    public class UserSettings
    {
        public string IdClaim { get; set; } = "sid";
        public string UsernameClaim { get; set; } = "username";
        public string EmailClaim { get; set; } = "email";
        
        public string StudentClaim { get; set; } = "hrEduPersonAffiliation";
        public string StudentClaimValue { get; set; } = "student";
        public string ProfesorClaim { get; set; } = "hrEduPersonAffiliation";
        public string ProfesorClaimValue { get; set; } = "djelatnik";

        public IList<string> ProfesorMailWhitelist { get; set; } = [];
        public IList<MentorSettingsEntry> Mentori { get; set; } = [];
    }

    public class MentorSettingsEntry
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

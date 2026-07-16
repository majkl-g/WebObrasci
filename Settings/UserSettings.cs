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
    }
}

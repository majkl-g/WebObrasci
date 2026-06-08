namespace WebObrasci1.Settings
{
    public class UserSettings
    {
        public string IdClaim { get; set; } = "sid";
        public string StudentRole { get; set; } = "student";
        public string ProfesorRole { get; set; } = "profesor";
        public string AdminRole { get; set; } = "admin";
        public string UsernameClaim { get; set; } = "username";
        public string EmailClaim { get; set; } = "email";
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebObrasci1.Models
{
    public class User : BaseEntity
    {
        public string ExternalId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string? Title { get; set; }
        public string? MentorName { get; set; }
        public string? MentorMail { get; set; }

    }
}

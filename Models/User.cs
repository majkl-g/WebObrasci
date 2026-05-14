namespace WebObrasci1.Models
{
    public class User
    {
        public int Id { get; set; }

        public string ExternalId { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public int? Age { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}

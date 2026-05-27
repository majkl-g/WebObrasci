using System.ComponentModel.DataAnnotations;

namespace WebObrasci1.Models
{
    public class User : BaseEntity
    {
        public string ExternalId { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public int? Age { get; set; }
    }
}

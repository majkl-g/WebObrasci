using System.ComponentModel.DataAnnotations;

namespace WebObrasci1.Models
{
    public class FormSubmission : BaseEntity
    {
        public int FormId { get; set; }
        public Form Form { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public string DataJson { get; set; } = "";

        public ICollection<FormSubmissionApproval> Approvals { get; set; } = new List<FormSubmissionApproval>();
    }
}

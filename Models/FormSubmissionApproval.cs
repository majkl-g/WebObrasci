namespace WebObrasci1.Models
{
    public class FormSubmissionApproval : BaseEntity
    {
        public int FormSubmissionId { get; set; }
        public string ApprovalFrom { get; set; } = "";
        public string ApprovalAsRole { get; set; } = "";
        public DateTime ApprovedAt { get; set; } = DateTime.UtcNow;
        public bool Denied { get; set; }
    }
}

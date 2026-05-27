namespace WebObrasci1.Models
{
    public class FormRequiredApprovals : BaseEntity
    {
        public int FormId { get; set; }
        public string ApprovalRole { get; set; } = "";
    }
}

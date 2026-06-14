
namespace WebObrasci1.Models
{
    public class Form : BaseEntity
    {
        public string Title { get; set; } = "";
        public ICollection<FormField> Fields { get; set; } = new List<FormField>();
        public ICollection<FormRequiredApprovals> RequiredApprovals { get; set; } = new List<FormRequiredApprovals>();
        public bool Enabled { get; set; }
    }
}

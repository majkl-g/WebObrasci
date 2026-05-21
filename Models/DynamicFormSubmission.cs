namespace WebObrasci1.Models
{
    public class DynamicFormSubmission
    {
        public int Id { get; set; }

        public int FormId { get; set; }
        public DynamicForm Form { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public string DataJson { get; set; } = "";
    }
}

namespace WebObrasci1.Models
{
    public class DynamicForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<DynamicFormField> Fields { get; set; } = new List<DynamicFormField>();
    }
}

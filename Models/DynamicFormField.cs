namespace WebObrasci1.Models
{
    public class DynamicFormField
    {
        public int Id { get; set; }
        public int DynamicFormId { get; set; }
        public DynamicForm? DynamicForm { get; set; } 

        public string Label { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";// text, number, select, etc.
        public bool Required { get; set; }
    }
}

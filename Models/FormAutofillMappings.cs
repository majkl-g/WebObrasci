namespace WebObrasci1.Models
{
    public class FormAutofillMapping : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Mapping { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }
}

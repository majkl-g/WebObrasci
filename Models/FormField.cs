using System.ComponentModel.DataAnnotations;

namespace WebObrasci1.Models
{
    public class FormField : BaseEntity
    {
        public int FormId { get; set; }
        public Form? Form { get; set; } 

        public string Label { get; set; } = "";
        public string Name { get; set; } = "";
        public FormFieldType Type { get; set; } = FormFieldType.Text;
        public bool Required { get; set; }

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public StringType? StringType { get; set; }
    }
}

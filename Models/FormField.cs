using System.ComponentModel.DataAnnotations;

namespace WebObrasci1.Models
{
    public class FormField : BaseEntity
    {
        public int FormId { get; set; }
        public Form? Form { get; set; } 

        public string Label { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";// text, number, select, etc.
        public bool Required { get; set; }
    }
}

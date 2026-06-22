using WebObrasci1.Models;

namespace WebObrasci1.Dto
{
    public class DtoForm
    {
        public string Title { get; set; } = string.Empty;
        public IList<DtoFormField> Fields { get; set; } = [];

        public static DtoForm FromForm(Form form)
        {
            var f = new DtoForm();
            f.Title = form.Title;
            f.Fields = form.Fields.Select(x => new DtoFormField(x)).ToList();
            return f;
        }
    }
}

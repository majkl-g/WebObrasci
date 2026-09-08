using System.Globalization;
using WebObrasci1.Models;

namespace WebObrasci1.Dto
{
    public class DtoFormField
    {
        public FormField Field { get; set; } = new();
        public string AutofillValue { get; set; } = string.Empty;
        public int? AutofillValueInt => int.TryParse(AutofillValue, out var p) ? p : null;
        public decimal? AutofillValueDec => decimal.TryParse(AutofillValue, out var p) ? p : null;
        public bool AutofillValueBool => bool.TryParse(AutofillValue, out var p) ? p : false;
        public DateTime? AutofillValueDate => ParseDate(AutofillValue);

        public DtoFormField(FormField formField)
        {
            Field = formField;
            Field.SelectValues = Field.SelectValues
                .OrderBy(x => x.Value)
                .ToList();
        }

        private static DateTime? ParseDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date;
            if (DateTime.TryParse(dateString, out date))
                return date;
            return null;
        }
    }
}

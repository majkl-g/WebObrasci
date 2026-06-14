using WebObrasci1.Models;

namespace WebObrasci1.Dto
{
    public class FormSubmissionsPage : DtoPage<FormSubmission>
    {
        public FormSubmissionsPage(IList<FormSubmission> pageData, int pageNumber, int pageSize, int totalPages) 
            : base(pageData, pageNumber, pageSize, totalPages)
        {
        }
    }
}

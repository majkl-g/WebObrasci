using WebObrasci1.Models;

namespace WebObrasci1.Dto
{
    public class FormsPage : DtoPage<Form>
    {
        public FormsPage(IList<Form> pageData, int pageNumber, int pageSize, int totalPages) 
            : base(pageData, pageNumber, pageSize, totalPages)
        {
        }
    }
}

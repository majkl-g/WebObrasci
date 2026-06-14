using WebObrasci1.Models;

namespace WebObrasci1.Dto
{
    public class FormSubmissionsPage(IList<FormSubmission> pageData, int pageNumber, int pageSize, int totalPages)
    {
        public IList<FormSubmission> PageData { get; set; } = pageData;
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int TotalPages { get; set; } = totalPages;

        public bool IsEmpty => PageData.Any() == false;
    }
}

namespace WebObrasci1.Dto
{
    public class DtoPage<T> where T : class
    {
        public IList<T> PageData { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public bool IsEmpty => PageData.Any() == false;

        public DtoPage(IList<T> pageData, int pageNumber, int pageSize, int totalPages)
        {
            PageData = pageData;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }
    }
}

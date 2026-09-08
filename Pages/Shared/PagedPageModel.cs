using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebObrasci1.Dto;

namespace WebObrasci1.Pages.Shared
{
    public abstract class PagedPageModel<T> : PageModel where T : class
    {
        public const int _pageSize = 10;
        protected virtual bool ShowBanner { get; } = false;

        public DtoPage<T> PageData { get; set; } = new([], 1, 1, 0);

        public abstract Task<(IList<T> Data, int Total)> GetPageDataAsync(int skip, int take);

        public async Task OnGetAsync([FromQuery] int? pageNumber = 1)
        {
            ViewData["ShowBanner"] = ShowBanner;

            var pageNumChecked = Math.Max(pageNumber ?? 1, 1);
            var skip = _pageSize * (pageNumChecked - 1);

            (var data, var total) = await GetPageDataAsync(skip, _pageSize);

            var totalPages = (int)Math.Ceiling((double)total / _pageSize);

            PageData = new DtoPage<T>(data, pageNumChecked, _pageSize, totalPages);
        }
    }
}
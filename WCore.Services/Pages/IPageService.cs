using System.Collections.Generic;
using WCore.Core;
using WCore.Core.Domain.Pages;

namespace WCore.Services.Pages
{
    public interface IPageService : IRepository<Page>
    {
        IPagedList<Page> GetAllByFilters(string searchValue = "",
            PageType? PageType = null,
            FooterLocation? FooterLocation = null,
            int? ParentId = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            bool? HomePage = null,
            bool? RedirectPage = null,
            int skip = 0,
            int take = int.MaxValue);
        List<Page> PagesWithBreadcrumb(List<Page> _pageList = null,
            int? ParentId = null,
            bool? ShowOn = null,
            bool? IsSub = false,
            string Title = "");

        public Page GetHomePage();
    }
}

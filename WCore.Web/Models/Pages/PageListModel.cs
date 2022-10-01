using System.Collections.Generic;
using WCore.Framework.Models;

namespace WCore.Web.Models.Pages
{
    public partial class PageListModel : BaseWCoreModel
    {
        public PageListModel()
        {
            PagingFilteringContext = new PagePagingFilteringModel();
            Pages = new List<PageModel>();
        }

        public int WorkingLanguageId { get; set; }
        public PagePagingFilteringModel PagingFilteringContext { get; set; }
        public List<PageModel> Pages { get; set; }
    }
}

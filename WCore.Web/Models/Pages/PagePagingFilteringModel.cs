using WCore.Core.Domain.Pages;
using WCore.Framework.UI.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCore.Web.Models.Pages
{

    public partial class PagePagingFilteringModel : BasePageableModel
    {
        #region Properties

        public string searchValue { get; set; }
        public PageType? PageType { get; set; }
        public FooterLocation? FooterLocation { get; set; }
        public int? ParentId { get; set; }
        public bool? HomePage { get; set; }
        public bool? RedirectPage { get; set; }
        public bool? WithEntityPages { get; set; }

        #endregion
    }
}

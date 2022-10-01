using WCore.Core.Domain.Pages;
using WCore.Framework.UI.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCore.Web.Models.Newses
{

    public partial class NewsPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }
        public int? NewsCategoryId { get; set; }
        public bool? IsArchived { get; set; }
        public int? DisplayOrder { get; set; }

        #endregion
    }
    public partial class NewsCategoryPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }

        #endregion
    }
    public partial class NewsImagePagingFilteringModel : BasePageableModel
    {
        #region Properties
        public int NewsId { get; set; }

        #endregion
    }
}

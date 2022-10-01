using WCore.Core.Domain.Pages;
using WCore.Framework.UI.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WCore.Web.Models.Teams
{

    public partial class TeamPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? TeamCategoryId { get; set; }
        public int? DisplayOrder { get; set; }

        #endregion
    }
    public partial class TeamCategoryPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }
        public int? ParentId { get; set; }

        #endregion
    }
}

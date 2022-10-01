using WCore.Core.Domain.Pages;
using WCore.Framework.UI.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCore.Core.Domain.Academies;

namespace WCore.Web.Models.Academies
{

    public partial class AcademyPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }
        public int? AcademyCategoryId { get; set; }
        public bool? IsArchived { get; set; }
        public int? DisplayOrder { get; set; }

        #endregion
    }
    public partial class AcademyCategoryPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }
        public int? ParentId { get; set; }

        #endregion
    }
    public partial class AcademyImagePagingFilteringModel : BasePageableModel
    {
        #region Properties
        public int AcademyId { get; set; }

        #endregion
    }
    public partial class AcademyFilePagingFilteringModel : BasePageableModel
    {
        #region Properties
        public int AcademyId { get; set; }

        #endregion
    }
    public partial class AcademyVideoPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public int AcademyId { get; set; }
        public AcademyVideoResource? AcademyVideoResource { get; set; }

        #endregion
    }
}

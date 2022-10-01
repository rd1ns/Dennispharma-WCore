using WCore.Framework.UI.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCore.Core.Domain.Galleries;

namespace WCore.Web.Models.Galleries
{

    public partial class GalleryPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public GalleryType GalleryType { get; set; }
        #endregion
    }
    public partial class GalleryImagePagingFilteringModel : BasePageableModel
    {
        #region Properties
        public GalleryModel Gallery { get; set; }
        public int GalleryId { get; set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.Congresses
{
    public class CongressImageModel : BaseWCoreEntityModel
    {
        #region Ctor
        public CongressImageModel()
        {
        }
        #endregion

        #region Properties

        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }
        public string Original { get; set; }

        public int DisplayOrder { get; set; }
        public int CongressId { get; set; }
        public virtual CongressModel Congress { get; set; }
        #endregion
    }
    public partial class CongressImageListModel : BaseWCoreModel
    {
        public CongressImageListModel()
        {
            PagingFilteringContext = new CongressImagePagingFilteringModel();
            CongressImages = new List<CongressImageModel>();
        }

        public int WorkingLanguageId { get; set; }
        public CongressImagePagingFilteringModel PagingFilteringContext { get; set; }
        public IList<CongressImageModel> CongressImages { get; set; }
    }
    public partial class CongressImagePagingFilteringModel : BasePageableModel
    {
        #region Properties
        public int CongressId { get; set; }
        #endregion
    }
}

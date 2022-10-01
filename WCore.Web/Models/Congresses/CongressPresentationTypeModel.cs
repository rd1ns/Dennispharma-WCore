using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.Congresses
{
    public class CongressPresentationTypeModel : BaseWCoreEntityModel
    {
        #region Ctor
        public CongressPresentationTypeModel()
        {
            CongressPresentations = new CongressPresentationListModel();
        }
        #endregion

        #region Properties
        public string Body { get; set; }
        public string Title { get; set; }

        public int CongressId { get; set; }
        public CongressModel Congress { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public CongressPresentationListModel CongressPresentations { get; set; }
        #endregion
    }
    public partial class CongressPresentationTypeListModel : BaseWCoreModel
    {
        public CongressPresentationTypeListModel()
        {
            PagingFilteringContext = new CongressPresentationTypePagingFilteringModel();
            CongressPresentationTypes = new List<CongressPresentationTypeModel>();
        }

        public int WorkingLanguageId { get; set; }
        public CongressPresentationTypePagingFilteringModel PagingFilteringContext { get; set; }
        public IList<CongressPresentationTypeModel> CongressPresentationTypes { get; set; }
    }
    public partial class CongressPresentationTypePagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }
        public int CongressId { get; set; }
        #endregion
    }
}

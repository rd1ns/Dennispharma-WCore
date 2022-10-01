using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.Congresses
{
    public class CongressPaperTypeModel : BaseWCoreEntityModel
    {
        #region Ctor
        public CongressPaperTypeModel()
        {
            CongressPapers = new CongressPaperListModel();
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
        public CongressPaperListModel CongressPapers { get; set; }
        #endregion
    }
    public partial class CongressPaperTypeListModel : BaseWCoreModel
    {
        public CongressPaperTypeListModel()
        {
            PagingFilteringContext = new CongressPaperTypePagingFilteringModel();
            CongressPaperTypes = new List<CongressPaperTypeModel>();
        }

        public int WorkingLanguageId { get; set; }
        public CongressPaperTypePagingFilteringModel PagingFilteringContext { get; set; }
        public IList<CongressPaperTypeModel> CongressPaperTypes { get; set; }
    }
    public partial class CongressPaperTypePagingFilteringModel : BasePageableModel
    {
        #region Properties
        public int CongressId { get; set; }
        public string Title { get; set; }
        #endregion
    }
}

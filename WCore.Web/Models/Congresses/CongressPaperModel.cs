using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.Congresses
{
    public class CongressPaperModel : BaseWCoreEntityModel
    {
        #region Ctor
        public CongressPaperModel()
        {
        }
        #endregion

        #region Properties
        public string Body { get; set; }
        public string Title { get; set; }

        public int CongressId { get; set; }
        public CongressModel Congress { get; set; }

        public int CongressPaperTypeId { get; set; }
        public CongressPaperTypeModel CongressPaperType { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        #endregion
    }
    public partial class CongressPaperListModel : BaseWCoreModel
    {
        public CongressPaperListModel()
        {
            PagingFilteringContext = new CongressPaperPagingFilteringModel();
            CongressPapers = new List<CongressPaperModel>();
        }

        public int WorkingLanguageId { get; set; }
        public CongressPaperPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<CongressPaperModel> CongressPapers { get; set; }
    }
    public partial class CongressPaperPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public int CongressId { get; set; }
        public int CongressPaperTypeId { get; set; }
        public string Title { get; set; }
        #endregion
    }
}

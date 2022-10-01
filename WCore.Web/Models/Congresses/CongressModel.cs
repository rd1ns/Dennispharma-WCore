using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.Congresses
{
    public class CongressModel : BaseWCoreEntityModel
    {
        #region Ctor
        public CongressModel()
        {
            CongressImages = new CongressImageListModel();
            CongressPaperTypes = new CongressPaperTypeListModel();
            CongressPresentationTypes  = new CongressPresentationTypeListModel();
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string MiniTitle { get; set; }
        public string Body { get; set; }
        public string ShortBody { get; set; }
        public string AwardWinningWorks { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public string SeName { get; set; }
        public PageTitleModel PageTitle { get; set; }
        public CongressPaperTypeListModel CongressPaperTypes { get; set; }
        public CongressPresentationTypeListModel CongressPresentationTypes { get; set; }
        public CongressImageListModel CongressImages { get; set; }
        #endregion
    }
    public partial class CongressListModel : BaseWCoreModel
    {
        public CongressListModel()
        {
            PagingFilteringContext = new CongressPagingFilteringModel();
            Congresses = new List<CongressModel>();
        }

        public int WorkingLanguageId { get; set; }
        public CongressPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<CongressModel> Congresses { get; set; }
    }
    public partial class CongressPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }
        public bool? IsArchived { get; set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using WCore.Framework.Models;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.Congresses
{
    public class CongressPresentationModel : BaseWCoreEntityModel
    {
        #region Ctor
        public CongressPresentationModel()
        {
        }
        #endregion

        #region Properties
        public string Body { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string OwnersName { get; set; }
        public string OwnersSurname { get; set; }
        public string FilePath { get; set; }

        public int CongressPresentationTypeId { get; set; }
        public CongressPresentationTypeModel CongressPresentationType { get; set; }

        public int CongressId { get; set; }
        public CongressModel Congress { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }

        #endregion
    }
    public partial class CongressPresentationListModel : BaseWCoreModel
    {
        public CongressPresentationListModel()
        {
            PagingFilteringContext = new CongressPresentationPagingFilteringModel();
            CongressPresentations = new List<CongressPresentationModel>();
        }

        public int WorkingLanguageId { get; set; }
        public CongressPresentationPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<CongressPresentationModel> CongressPresentations { get; set; }
    }
    public partial class CongressPresentationPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public string Title { get; set; }
        public int CongressPresentationTypeId { get; set; }
        public int CongressId { get; set; }
        #endregion
    }
}

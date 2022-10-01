using System.Collections.Generic;
using WCore.Core.Domain.DynamicForms;
using WCore.Framework.Models;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.DynamicForms
{
    /// <summary>
    /// Represents a dynamic form list model
    /// </summary>
    public partial class DynamicFormListModel : BaseWCoreModel
    {
        public DynamicFormListModel()
        {
            PagingFilteringContext = new DynamicFormPagingFilteringModel();
            DynamicForms = new List<DynamicFormModel>();
        }

        public int WorkingLanguageId { get; set; }

        public DynamicFormPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<DynamicFormModel> DynamicForms { get; set; }
    }
}

namespace WCore.Web.Models.DynamicForms
{
    /// <summary>
    /// Represents a dynamic form list model
    /// </summary>
    public partial class DynamicFormElementListModel : BaseWCoreModel
    {

        public DynamicFormElementListModel()
        {
            PagingFilteringContext = new DynamicFormElementPagingFilteringModel();
            DynamicFormElements = new List<DynamicFormElementModel>();
        }

        public int WorkingLanguageId { get; set; }
        public DynamicFormElementPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<DynamicFormElementModel> DynamicFormElements { get; set; }
    }
}
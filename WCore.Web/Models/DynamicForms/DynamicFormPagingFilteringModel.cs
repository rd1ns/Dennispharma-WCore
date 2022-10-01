using WCore.Core.Domain.DynamicForms;
using WCore.Framework.UI.Paging;

namespace WCore.Web.Models.DynamicForms
{

    public partial class DynamicFormPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public DynamicFormType? DynamicFormType { get; set; }
        #endregion
    }
    public partial class DynamicFormElementPagingFilteringModel : BasePageableModel
    {
        #region Properties
        public DynamicFormModel DynamicForm { get; set; }
        public int DynamicFormId { get; set; }
        #endregion
    }
}

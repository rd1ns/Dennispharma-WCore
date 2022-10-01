using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Pages;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System;

namespace WCore.Web.Areas.Admin.Models.DynamicForms
{
    public partial class DynamicFormModel : BaseWCoreEntityModel, ILocalizedModel<DynamicFormLocalizedModel>
    {
        #region Ctor
        public DynamicFormModel()
        {
            Locales = new List<DynamicFormLocalizedModel>();
            DynamicFormTypes = new List<SelectListItem>();
            DynamicFormElements = new List<DynamicFormElementModel>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm.Result")]
        public string Result { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm.ToAddresses")]
        public string ToAddresses { get; set; }


        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DynamicFormType")]
        public DynamicFormType DynamicFormType { get; set; }
        public string DynamicFormTypeName { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.UseLocalization")]
        public bool UseLocalization { get; set; }

        public IList<DynamicFormLocalizedModel> Locales { get; set; }
        public List<SelectListItem> DynamicFormTypes { get; set; }
        public List<DynamicFormElementModel> DynamicFormElements { get; set; }

        #endregion
    }
    public partial class DynamicFormLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm.Result")]
        public string Result { get; set; }
    }
    public partial class DynamicFormSearchModel : BaseSearchModel
    {
        #region Ctor

        public DynamicFormSearchModel()
        {
            DynamicFormTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DynamicFormType")]
        public DynamicFormType? DynamicFormType { get; set; }
        public List<SelectListItem> DynamicFormTypes { get; set; }

        #endregion
    }
}

namespace WCore.Web.Areas.Admin.Models.DynamicForms
{
    public partial class DynamicFormElementModel : BaseWCoreEntityModel
    {
        #region Ctor
        public DynamicFormElementModel()
        {
            DynamicForms = new List<SelectListItem>();
            ControlElements = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.ControlElement")]
        public ControlElement ControlElement { get; set; }
        public string ControlElementName { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.TiControlValuetle")]
        public string ControlValue { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ControlLabel")]
        public string ControlLabel { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Required")]
        public bool Required { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm")]
        public int DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        public List<SelectListItem> DynamicForms { get; set; }
        public List<SelectListItem> ControlElements { get; set; }

        #endregion
    }
    public partial class DynamicFormElementSearchModel : BaseSearchModel
    {
        #region Ctor

        public DynamicFormElementSearchModel()
        {
            DynamicForms = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm")]
        public int DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        public List<SelectListItem> DynamicForms { get; set; }

        #endregion
    }
}

namespace WCore.Web.Areas.Admin.Models.DynamicForms
{
    public partial class DynamicFormRecordModel : BaseWCoreEntityModel
    {
        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm")]
        public int DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        #endregion
    }
    public partial class DynamicFormRecordSearchModel : BaseSearchModel
    {
        #region Properties
        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm")]
        public int DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        #endregion
    }
}
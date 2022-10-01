using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Pages;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System;

namespace WCore.Web.Models.DynamicForms
{
    public partial class DynamicFormModel : BaseWCoreEntityModel
    {
        #region Ctor
        public DynamicFormModel()
        {
            DynamicFormTypes = new List<SelectListItem>();
            DynamicFormElements = new List<DynamicFormElementModel>();
        }
        #endregion

        #region Properties

        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Result { get; set; }
        public string ToAddresses { get; set; }


        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public DynamicFormType DynamicFormType { get; set; }
        public string DynamicFormTypeName { get; set; }
        public bool UseLocalization { get; set; }
        public List<SelectListItem> DynamicFormTypes { get; set; }
        public List<DynamicFormElementModel> DynamicFormElements { get; set; }

        #endregion
    }

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

        public ControlElement ControlElement { get; set; }
        public string ControlElementName { get; set; }
        public int DisplayOrder { get; set; }
        public string ControlValue { get; set; }
        public string ControlLabel { get; set; }
        public bool Required { get; set; }

        public int DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        public List<SelectListItem> DynamicForms { get; set; }
        public List<SelectListItem> ControlElements { get; set; }

        #endregion
    }

    public partial class DynamicFormRecordModel : BaseWCoreEntityModel
    {
        #region Properties

        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public int DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        #endregion
    }
}
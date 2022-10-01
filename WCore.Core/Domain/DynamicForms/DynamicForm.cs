using System;
using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.DynamicForms
{
    public partial class DynamicForm : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string ToAddresses { get; set; }
        public DynamicFormType DynamicFormType { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public bool UseLocalization { get; set; }
        public string Result { get; set; }
    }
    public partial class DynamicFormElement : BaseEntity
    {
        public ControlElement ControlElement { get; set; }
        public string ControlValue { get; set; }
        public string ControlLabel { get; set; }
        public bool Required { get; set; }
        public int DisplayOrder { get; set; }
        public int DynamicFormId { get; set; }
        public virtual DynamicForm DynamicForm { get; set; }
    }
    public partial class DynamicFormRecord : BaseEntity
    {
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }

        public int DynamicFormId { get; set; }
        public virtual DynamicForm DynamicForm { get; set; }
    }
    public enum ControlElement
    {
        List = 0,
        Textbox = 1,
        Number = 2,
        Checkbox = 3,
        Radiobutton = 4,
        Textarea = 5,
        Email = 6,
        FileUpload = 7
    }
    public enum DynamicFormType
    {
        Default = 0,
        Contact = 1,
        Offer = 2,
        Appointment = 3,
        MembershipApplication = 4
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using WCore.Core.Domain.Users;

namespace WCore.Core.Domain.Templates
{
    public class Template : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public TemplateType TemplateType { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CreatedUserId { get; set; }
        public virtual User CreatedUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemTemplate { get; set; }
    }
    public enum TemplateType
    {
        [Display(Name = "Yazı")]
        Article = 0,
        [Display(Name = "Teklif")]
        Bid = 1,
        [Display(Name = "Yeni Üyelik")]
        NewRegister = 2,
        [Display(Name = "Şifre Hatırlatma")]
        ForgotPassword = 3,
        [Display(Name = "Randevu Hatırlatma")]
        ActivityReminder = 4,
        [Display(Name = "Yeni Firma Kaydı")]
        NewCompanyRecord = 5,
        [Display(Name = "Yeni Sipariş Kaydı")]
        NewOrderRecord = 6,

    }
}

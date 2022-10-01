using System;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Seo;

namespace WCore.Core.Domain.Newses
{
    public partial class News : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        public int NewsCategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; }

        public int DisplayOrder { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public bool ShowOnHome { get; set; }
    }
}

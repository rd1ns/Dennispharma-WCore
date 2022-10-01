using System;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Seo;

namespace WCore.Core.Domain.Congresses
{
    public partial class Congress : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        public string Title { get; set; }
        public string MiniTitle { get; set; }
        public string Body { get; set; }
        public string ShortBody { get; set; }
        public string AwardWinningWorks { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }

        public int DisplayOrder { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
}

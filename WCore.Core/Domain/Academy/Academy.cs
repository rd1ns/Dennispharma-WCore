using System;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Seo;

namespace WCore.Core.Domain.Academies
{
    public partial class Academy : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }

        public int AcademyCategoryId { get; set; }
        public AcademyCategory AcademyCategory { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
}

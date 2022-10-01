using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Seo;

namespace WCore.Core.Domain.Teams
{
    public partial class TeamCategory : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        public string Title { get; set; }

        public int ParentId { get; set; }
        public TeamCategory Parent { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
}

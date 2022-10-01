using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Newses
{
    public partial class NewsImage : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }
        public string Original { get; set; }

        public int NewsId { get; set; }
        public virtual News News { get; set; }

        public int DisplayOrder { get; set; }
    }
}

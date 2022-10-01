using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Congresses
{
    public partial class CongressImage : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }
        public string Original { get; set; }

        public int CongressId { get; set; }
        public virtual Congress Congress { get; set; }

        public int DisplayOrder { get; set; }
    }
}

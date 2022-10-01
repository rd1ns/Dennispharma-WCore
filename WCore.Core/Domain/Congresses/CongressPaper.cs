using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Seo;

namespace WCore.Core.Domain.Congresses
{
    public partial class CongressPaper : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Code { get; set; }
        public string OwnersName { get; set; }
        public string OwnersSurname { get; set; }
        public string FilePath { get; set; }

        public int CongressId { get; set; }
        public Congress Congress { get; set; }

        public int CongressPaperTypeId { get; set; }
        public CongressPaperType CongressPaperType { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
}

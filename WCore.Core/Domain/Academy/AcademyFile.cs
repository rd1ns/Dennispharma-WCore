using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Academies
{
    public partial class AcademyFile : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Path { get; set; }

        public int AcademyId { get; set; }
        public virtual Academy Academy { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
}

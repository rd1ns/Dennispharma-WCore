using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Academies
{
    public partial class AcademyVideo : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public AcademyVideoResource AcademyVideoResource { get; set; }

        public int AcademyId { get; set; }
        public virtual Academy Academy { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
    public enum AcademyVideoResource
    {
        WebSite = 0,
        Youtube = 1,
        Vimeo = 2
    }
}

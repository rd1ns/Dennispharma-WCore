using WCore.Core.Domain.Localization;

namespace WCore.Core.Domain.Galleries
{
    public partial class Gallery : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ShortBody { get; set; }
        public string Image { get; set; }
        public GalleryType GalleryType { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
    public partial class GalleryImage : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }
        public string Original { get; set; }
        public string VideoLink { get; set; }
        public bool IsVideo { get; set; }

        public int DisplayOrder { get; set; }

        public int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
    public enum GalleryType
    {
        Single = 0,
        Group = 1,
        Slider = 2,
        BottomSlider = 3,
        HomeMiniGallery = 4,
    }
}

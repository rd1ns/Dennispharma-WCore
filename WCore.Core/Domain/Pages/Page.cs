using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Seo;

namespace WCore.Core.Domain.Pages
{
    public partial class Page : BaseEntity, ILocalizedEntity, ISlugSupported
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string LeftBody { get; set; }
        public string RightBody { get; set; }
        public string ShortBody { get; set; }
        public string Image { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public int EntityId { get; set; }

        public int? ParentId { get; set; }
        public virtual Page Parent { get; set; }

        public int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }

        public int DynamicFormId { get; set; }
        public virtual DynamicForm DynamicForm { get; set; }

        public PageType PageType { get; set; }
        public FooterLocation FooterLocation { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool HomePage { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }

        public bool RedirectPage { get; set; }
        public string RedirectPageUrl { get; set; }
    }
    public enum PageType
    {
        Default = 0,
        News = 1,
        Contact = 2,
        Teams = 3,
        Academy = 4,
        Congress = 5,
        Form = 6,
        SubContent = 99,
        ScrollContent = 100,
        //SkiResortList = 3,
        //HotelList = 4,
        //TourCategoryList = 5,
        //TourList = 6,
        //SkiSchoolList = 7,
        //RestaurantList = 8
    }
    public enum FooterLocation
    {
        None = 0,
        First = 1,
        Second = 2,
        Bottom = 30
    }
}

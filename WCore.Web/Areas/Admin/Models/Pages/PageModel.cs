using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core.Domain.Pages;
using WCore.Framework.Models;
using WCore.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using WCore.Web.Areas.Admin.Models.DynamicForms;
using WCore.Web.Areas.Admin.Models.Galleries;

namespace WCore.Web.Areas.Admin.Models.Pages
{
    public partial class PageModel : BaseWCoreEntityModel, ILocalizedModel<PageLocalizedModel>
    {
        #region Ctor
        public PageModel()
        {
            Locales = new List<PageLocalizedModel>();
            Parents = new List<SelectListItem>();
            PageTypes = new List<SelectListItem>();
            FooterLocations = new List<SelectListItem>();
            Galleries = new List<SelectListItem>();
            DynamicForms = new List<SelectListItem>();
        }
        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.LeftBody")]
        public string LeftBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.RightBody")]
        public string RightBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShortBody")]
        public string ShortBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Image")]
        public string Image { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaKeywords")]
        public string MetaKeywords { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaDescription")]
        public string MetaDescription { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaTitle")]
        public string MetaTitle { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Entity")]
        public int EntityId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Parent")]
        public int ParentId { get; set; }
        public virtual PageModel Parent { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DynamicForm")]
        public int DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Gallery")]
        public int GalleryId { get; set; }
        public virtual GalleryModel Gallery { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.PageType")]
        public PageType PageType { get; set; }
        public string PageTypeName { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.FooterLocation")]
        public FooterLocation FooterLocation { get; set; }
        public string FooterLocationName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.HomePage")]
        public bool HomePage { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool ShowOn { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.RedirectPage")]
        public bool RedirectPage { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.RedirectPageUrl")]
        public string RedirectPageUrl { get; set; }
        public IList<PageLocalizedModel> Locales { get; set; }
        public List<SelectListItem> Parents { get; set; }
        public List<SelectListItem> PageTypes { get; set; }
        public List<SelectListItem> FooterLocations { get; set; }
        public List<SelectListItem> Galleries { get; set; }
        public List<SelectListItem> DynamicForms { get; set; }

        #endregion
    }

    public partial class PageLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.Title")]
        public string Title { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Body")]
        public string Body { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.LeftBody")]
        public string LeftBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.RightBody")]
        public string RightBody { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShortBody")]
        public string ShortBody { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.MetaKeywords")]
        public string MetaKeywords { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaDescription")]
        public string MetaDescription { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.RedirectPageUrl")]
        public string RedirectPageUrl { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.MetaTitle")]
        public string MetaTitle { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.SeName")]
        public string SeName { get; set; }
    }
    public partial class PageSearchModel : BaseSearchModel
    {
        #region Ctor

        public PageSearchModel()
        {
            Parents = new List<SelectListItem>();
            PageTypes = new List<SelectListItem>();
            FooterLocations = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [WCoreResourceDisplayName("Admin.Configuration.Query")]
        public string Query { get; set; }


        [WCoreResourceDisplayName("Admin.Configuration.PageType")]
        public PageType? PageType { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.FooterLocation")]
        public FooterLocation? FooterLocation { get; set; }

        [WCoreResourceDisplayName("Admin.Configuration.IsActive")]
        public bool? IsActive { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Deleted")]
        public bool? Deleted { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.ShowOn")]
        public bool? ShowOn { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.HomePage")]
        public bool? HomePage { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.Parent")]
        public int? ParentId { get; set; }
        [WCoreResourceDisplayName("Admin.Configuration.RedirectPage")]
        public bool? RedirectPage { get; set; }

        public List<SelectListItem> Parents { get; set; }
        public List<SelectListItem> PageTypes { get; set; }
        public List<SelectListItem> FooterLocations { get; set; }
        #endregion
    }
}

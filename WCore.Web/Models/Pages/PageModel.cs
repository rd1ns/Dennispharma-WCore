using System.Collections.Generic;
using WCore.Core.Domain.Pages;
using WCore.Framework.Models;
using WCore.Web.Models.DynamicForms;
using WCore.Web.Models.Galleries;

namespace WCore.Web.Models.Pages
{
    public class PageModel : BaseWCoreEntityModel
    {
        #region Ctor
        public PageModel()
        {
            SubPages = new List<PageModel>();
        }
        #endregion

        #region Properties

        public string Title { get; set; }
        public string Body { get; set; }
        public string LeftBody { get; set; }
        public string RightBody { get; set; }
        public string ShortBody { get; set; }
        public string Image { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public bool RedirectPage { get; set; }
        public string RedirectPageUrl { get; set; }

        public int EntityId { get; set; }
        public int? GalleryId { get; set; }
        public virtual GalleryModel Gallery { get; set; }
        public int? DynamicFormId { get; set; }
        public virtual DynamicFormModel DynamicForm { get; set; }

        public int? ParentId { get; set; }
        public virtual PageModel Parent { get; set; }

        public PageType PageType { get; set; }
        public FooterLocation FooterLocation { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool HomePage { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }

        public string SeName { get; set; }


        public PageTitleModel PageTitle { get; set; }
        public List<PageModel> SubPages { get; set; }
        #endregion
    }
}

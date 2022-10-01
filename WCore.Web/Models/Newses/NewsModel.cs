using System;
using System.Collections.Generic;
using WCore.Framework.Models;

namespace WCore.Web.Models.Newses
{
    public class NewsModel : BaseWCoreEntityModel
    {
        #region Ctor
        public NewsModel()
        {
            NewsImages = new NewsImageListModel();
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        public int NewsCategoryId { get; set; }
        public virtual NewsCategoryModel NewsCategory { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsArchived { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public string SeName { get; set; }


        public PageTitleModel PageTitle { get; set; }
        public NewsImageListModel NewsImages { get;set;}
        #endregion
    }
    public class NewsCategoryModel : BaseWCoreEntityModel
    {
        #region Ctor
        public NewsCategoryModel()
        {
            Newses = new NewsListModel();
        }
        #endregion

        #region Properties
        public string Body { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public bool ShowOnHome { get; set; }
        public string SeName { get; set; }

        public NewsListModel Newses { get; set; }


        public PageTitleModel PageTitle { get; set; }
        #endregion
    }
    public class NewsImageModel : BaseWCoreEntityModel
    {
        #region Ctor
        public NewsImageModel()
        {
        }
        #endregion

        #region Properties

        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }
        public string Original { get; set; }

        public int DisplayOrder { get; set; }

        public int NewsId { get; set; }
        public virtual NewsModel News { get; set; }
        #endregion
    }
}

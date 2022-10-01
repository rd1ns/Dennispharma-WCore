using System.Collections.Generic;
using WCore.Core.Domain.Galleries;
using WCore.Framework.Models;

namespace WCore.Web.Models.Galleries
{
    public class GalleryModel : BaseWCoreEntityModel
    {
        #region Ctor
        public GalleryModel()
        {
            GalleryImages = new List<GalleryImageModel>();
        }
        #endregion

        #region Properties

        public string Title { get; set; }
        public string Body { get; set; }
        public string ShortBody { get; set; }
        public string Image { get; set; }
        public GalleryType GalleryType { get; set; }
        public string GalleryTypeName { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public List<GalleryImageModel> GalleryImages { get; set; }
        #endregion
    }
    public class GalleryImageModel : BaseWCoreEntityModel
    {
        #region Properties

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
        public virtual GalleryModel Gallery { get; set; }

        #endregion
    }
}

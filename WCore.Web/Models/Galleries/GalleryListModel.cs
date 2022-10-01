using System.Collections.Generic;
using WCore.Framework.Models;

namespace WCore.Web.Models.Galleries
{
    public partial class GalleryListModel : BaseWCoreModel
    {
        public GalleryListModel()
        {
            PagingFilteringContext = new GalleryPagingFilteringModel();
            Galleries = new List<GalleryModel>();
        }

        public int WorkingLanguageId { get; set; }
        public GalleryPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<GalleryModel> Galleries { get; set; }
    }
    public partial class GalleryImageListModel : BaseWCoreModel
    {
        public GalleryImageListModel()
        {
            PagingFilteringContext = new GalleryImagePagingFilteringModel();
            GalleryImages = new List<GalleryImageModel>();
        }

        public int WorkingLanguageId { get; set; }
        public GalleryImagePagingFilteringModel PagingFilteringContext { get; set; }
        public IList<GalleryImageModel> GalleryImages { get; set; }
    }
}

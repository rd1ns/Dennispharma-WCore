using System.Collections.Generic;
using WCore.Web.Models.Galleries;

namespace WCore.Web.Models.ViewModels
{
    public class GalleryViewModel
    {
        public GalleryViewModel()
        {
            GalleryImages = new List<GalleryImageModel>();
        }
        public virtual GalleryModel  Gallery { get; set; }
        public virtual List<GalleryImageModel>  GalleryImages { get; set; }
    }
}

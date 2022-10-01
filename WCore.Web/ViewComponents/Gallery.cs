using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Localization;
using WCore.Services.Users;
using WCore.Web.Factories;

namespace WCore.Web.ViewComponents
{
    public class GalleryViewComponent : ViewComponent
    {
        private readonly IGalleryModelFactory _galleryModelFactory;
        private readonly IGalleryImageModelFactory _galleryImageModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public GalleryViewComponent(IGalleryModelFactory galleryModelFactory,
            IGalleryImageModelFactory galleryImageModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._galleryModelFactory = galleryModelFactory;
            this._galleryImageModelFactory = galleryImageModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke(int galleryId)
        {
            var model = _galleryModelFactory.PrepareGalleryModel(galleryId);
            return View(model);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Localization;
using WCore.Web.Factories;

namespace WCore.Web.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly IGalleryModelFactory _galleryModelFactory;
        private readonly IGalleryImageModelFactory _galleryImageModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public SliderViewComponent(IGalleryModelFactory galleryModelFactory,
            IGalleryImageModelFactory galleryImageModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._galleryModelFactory = galleryModelFactory;
            this._galleryImageModelFactory = galleryImageModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke()
        {
            var model = _galleryModelFactory.PrepareGalleryModel(Core.Domain.Galleries.GalleryType.Slider);
            return View(model);
        }
    }
}

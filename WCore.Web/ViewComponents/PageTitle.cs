using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Localization;
using WCore.Web.Factories;
using WCore.Web.Models;
using WCore.Web.Models.ViewModels;

namespace WCore.Web.ViewComponents
{
    public class PageTitleViewComponent : ViewComponent
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPageModelFactory _pageModelFactory;
        private readonly IWorkContext _workContext;
        public PageTitleViewComponent(ILocalizationService localizationService,
            IPageModelFactory pageModelFactory,
            IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._pageModelFactory = pageModelFactory;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke(PageTitleModel pageTitleModel)
        {
            var model = new PageTitleViewModel();

            model.PageTitle = pageTitleModel;

            return View(model);
        }
    }
}

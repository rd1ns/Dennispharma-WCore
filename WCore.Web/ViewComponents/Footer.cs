using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Framework.Extensions;
using WCore.Services.Localization;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.ViewModels;
using System.Linq;

namespace WCore.Web.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPageModelFactory _pageModelFactory;
        private readonly IWorkContext _workContext;
        public FooterViewComponent(ILocalizationService localizationService,
            IPageModelFactory pageModelFactory,
            IWorkContext workContext)
        {
            this._localizationService = localizationService;
            this._pageModelFactory = pageModelFactory;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke()
        {
            var model = new FooterViewModel();

            var pagePageingFiltering = new Models.Pages.PagePagingFilteringModel()
            {
                IsActive = true,
                Deleted = false,
                PageNumber = 1,
                PageSize = int.MaxValue
            };
            var list = _pageModelFactory.PreparePageListModel(pagePageingFiltering);

            model.PageList = list;

            return View(model);
        }
    }
}

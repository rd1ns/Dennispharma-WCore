using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Congresses;
using WCore.Services.Localization;
using WCore.Services.Newses;
using WCore.Services.Users;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Congresses;
using WCore.Web.Models.Newses;

namespace WCore.Web.ViewComponents
{
    public class CongressViewComponent : ViewComponent
    {
        private readonly ICongressService _congressService;
        private readonly ICongressModelFactory _congressModelFactory;
        private readonly INewsService _newsService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public CongressViewComponent(ICongressService congressService,
            ICongressModelFactory congressModelFactory,
            INewsService newsService,
            INewsModelFactory newsModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._congressService = congressService;
            this._congressModelFactory = congressModelFactory;
            this._newsService = newsService;
            this._newsModelFactory = newsModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke()
        {
            var congressPaging = new CongressPagingFilteringModel() { IsActive = true, Deleted = false,PageSize=100, PageNumber=1 };
            var congreses = _congressModelFactory.PrepareCongressListModel(congressPaging);
            return View(congreses);
        }
    }
}

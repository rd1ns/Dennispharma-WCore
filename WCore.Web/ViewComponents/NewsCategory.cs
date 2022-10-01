using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Services.Localization;
using WCore.Services.Newses;
using WCore.Services.Users;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Newses;

namespace WCore.Web.ViewComponents
{
    public class NewsCategoryViewComponent : ViewComponent
    {
        private readonly INewsCategoryService _newsCategoryService;
        private readonly INewsCategoryModelFactory _newsCategoryModelFactory;
        private readonly INewsService _newsService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public NewsCategoryViewComponent(INewsCategoryService newsCategoryService,
            INewsCategoryModelFactory newsCategoryModelFactory,
            INewsService newsService,
            INewsModelFactory newsModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._newsCategoryService = newsCategoryService;
            this._newsCategoryModelFactory = newsCategoryModelFactory;
            this._newsService = newsService;
            this._newsModelFactory = newsModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke(int newsCategoryId)
        {
            var newsCategory = _newsCategoryService.GetById(newsCategoryId);

            var model = newsCategory.ToModel<NewsCategoryModel>();

            if (model == null)
            {
                model = new NewsCategoryModel();
                var allNewsCategories = _newsCategoryService.GetAllByFilters(IsActive: true, Deleted: false, ShowOn: true);
                foreach (var item in allNewsCategories)
                {
                    var _newsPaging = new NewsPagingFilteringModel()
                    {
                        NewsCategoryId = item.Id
                    };
                    if (_newsModelFactory.PrepareNewsListModel(_newsPaging).Newses.Any())
                    {
                        model.Newses.Newses.AddRange(_newsModelFactory.PrepareNewsListModel(_newsPaging).Newses);
                    }
                }
            }
            else
            {
                var newsPaging = new NewsPagingFilteringModel()
                {
                    NewsCategoryId = newsCategoryId
                };
                model.Newses = _newsModelFactory.PrepareNewsListModel(newsPaging);
            }
            return View(model);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Newses;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Newses;
using WCore.Web.Models.ViewModels;

namespace WCore.Web.Controllers
{
    public class NewsController : BasePublicController
    {
        #region Fields
        private readonly INewsService _newsService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly INewsCategoryService _newsCategoryService;
        private readonly INewsCategoryModelFactory _newsCategoryModelFactory;
        private readonly INewsImageModelFactory _newsImageModelFactory;


        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public NewsController(INewsService newsService,
            INewsModelFactory newsModelFactory,
            INewsCategoryService newsCategoryService,
            INewsCategoryModelFactory newsCategoryModelFactory,
            INewsImageModelFactory newsImageModelFactory,
            IUserModelFactory userModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._newsService = newsService;
            this._newsModelFactory = newsModelFactory;
            this._newsCategoryService = newsCategoryService;
            this._newsCategoryModelFactory = newsCategoryModelFactory;
            this._newsImageModelFactory = newsImageModelFactory;

            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Details(int newsid)
        {
            var news = _newsService.GetById(newsid, cache => default);
            if (news == null)
                return InvokeHttp404();

            if ((news.Deleted || !news.IsActive) && _workContext.CurrentUser.RoleGroup.RoleGroupType != Core.Domain.Roles.RoleGroupType.WCore)
                return NotFound();

            var model = new NewsViewModel();
            if (news != null)
            {
                model.News = news.ToModel<NewsModel>();
                _newsModelFactory.PrepareNewsModel(model.News, news);

                model.News.PageTitle = new Models.PageTitleModel()
                {
                    Title = model.News.Title,
                    Image = model.News.Image
                };

                if (model.News != null)
                {
                    model.News.NewsImages = _newsImageModelFactory.PrepareNewsImageListModel(new NewsImagePagingFilteringModel() { NewsId = newsid });
                }

                model.NewsCategoryList = _newsCategoryModelFactory.PrepareNewsCategoryListModel(new NewsCategoryPagingFilteringModel() { });
                model.RecentNewsList = _newsModelFactory.PrepareNewsListModel(new NewsPagingFilteringModel() { NewsCategoryId = news.NewsCategoryId });
            }

            return View(model);
        }
        #endregion
    }
}

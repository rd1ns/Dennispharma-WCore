using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Settings;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Users;
using WCore.Web.Models.ViewModels;

namespace WCore.Web.Controllers
{
    public class HomeController : BasePublicController
    {
        #region 
        private readonly IPageModelFactory _pageModelFactory;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public HomeController(IPageModelFactory pageModelFactory,
            INewsModelFactory newsModelFactory,
            IUserModelFactory userModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._pageModelFactory = pageModelFactory;
            this._newsModelFactory= newsModelFactory;
            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            var model = new HomeViewModel();

            var user = _workContext.CurrentUser.ToModel<UserModel>();
            _userModelFactory.PrepareUserModel(user, _workContext.CurrentUser);
            model.User = user;

            model.Page = _pageModelFactory.PreparePageModel(homePage: true);

            model.NewsList = _newsModelFactory.PrepareNewsListModel(new Models.Newses.NewsPagingFilteringModel() { ShowOnHome = true });

            return View(model);
        }
        #endregion
    }
}

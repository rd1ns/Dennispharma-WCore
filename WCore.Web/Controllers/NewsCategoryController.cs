
using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Newses;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Newses;

namespace WCore.Web.Controllers
{
    public class NewsCategoryController : BasePublicController
    {
        #region Fields
        private readonly INewsCategoryService _newsCategoryService;
        private readonly INewsCategoryModelFactory _newsCategoryModelFactory;
        private readonly INewsModelFactory _newsModelFactory;


        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public NewsCategoryController(INewsCategoryService newsCategoryService,
            INewsCategoryModelFactory newsCategoryModelFactory,
            INewsModelFactory newsModelFactory,
            IUserModelFactory userModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._newsCategoryService = newsCategoryService;
            this._newsCategoryModelFactory = newsCategoryModelFactory;
            this._newsModelFactory = newsModelFactory;

            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Details(int newsCategoryid)
        {
            var newsCategory = _newsCategoryService.GetById(newsCategoryid, cache => default);
            if (newsCategory == null)
                return InvokeHttp404();

            if ((newsCategory.Deleted || !newsCategory.IsActive) && _workContext.CurrentUser.RoleGroup.RoleGroupType != Core.Domain.Roles.RoleGroupType.WCore)
                return NotFound();

            var model = newsCategory.ToModel<NewsCategoryModel>();
            _newsCategoryModelFactory.PrepareNewsCategoryModel(model, newsCategory);

            model.PageTitle = new Models.PageTitleModel()
            {
                Title = model.Title,
                Image = model.Image
            };


            var newsPaging = new NewsPagingFilteringModel()
            {
                NewsCategoryId = newsCategoryid
            };
            model.Newses = _newsModelFactory.PrepareNewsListModel(newsPaging);

            return View(model);
        }
        #endregion
    }
}

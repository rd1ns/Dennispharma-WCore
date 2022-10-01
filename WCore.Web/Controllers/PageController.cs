using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Pages;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Pages;

namespace WCore.Web.Controllers
{
    public class PageController : BasePublicController
    {
        #region Fields
        private readonly IPageService _pageService;
        private readonly IPageModelFactory _pageModelFactory;


        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public PageController(IPageService pageService,
            IPageModelFactory pageModelFactory,
            IUserModelFactory userModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._pageService = pageService;
            this._pageModelFactory = pageModelFactory;

            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Details(int pageid)
        {
            var page = _pageService.GetById(pageid, cache => default);
            if (page == null)
                return InvokeHttp404();

            //switch (page.PageType)
            //{
            //    case Core.Domain.Pages.PageType.SkiResortList:
            //        return RedirectToRoute("SkiResortList");
            //    case Core.Domain.Pages.PageType.HotelList:
            //        return RedirectToRoute("HotelList");
            //    case Core.Domain.Pages.PageType.TourCategoryList:
            //        return RedirectToRoute("TourCategoryList");
            //    case Core.Domain.Pages.PageType.TourList:
            //        return RedirectToRoute("TourList");
            //    case Core.Domain.Pages.PageType.SkiSchoolList:
            //        return RedirectToRoute("SkiSchoolList");
            //    case Core.Domain.Pages.PageType.RestaurantList:
            //        return RedirectToRoute("RestaurantList");
            //}

            if ((page.Deleted || !page.IsActive) /*&& _workContext.CurrentUser.RoleGroup.RoleGroupType != Core.Domain.Roles.RoleGroupType.WCore*/)
                return NotFound();

            var model = page.ToModel<PageModel>();
            _pageModelFactory.PreparePageModel(model, page);

            model.PageTitle = new Models.PageTitleModel()
            {
                Title = model.Title,
                ShortTitle = model.ShortBody,
                Image = model.Image
            };


            var pagePaging = new PagePagingFilteringModel()
            {
                ParentId = pageid
            };
            model.SubPages = _pageModelFactory.PreparePageListModel(pagePaging).Pages;

            return View(model);
        }
        #endregion
    }
}

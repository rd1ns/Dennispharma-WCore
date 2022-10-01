
using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Academies;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Academies;

namespace WCore.Web.Controllers
{
    public class AcademyCategoryController : BasePublicController
    {
        #region Fields
        private readonly IAcademyCategoryService _academyCategoryService;
        private readonly IAcademyCategoryModelFactory _academyCategoryModelFactory;
        private readonly IAcademyModelFactory _academyModelFactory;


        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public AcademyCategoryController(IAcademyCategoryService academyCategoryService,
            IAcademyCategoryModelFactory academyCategoryModelFactory,
            IAcademyModelFactory academyModelFactory,
            IUserModelFactory userModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._academyCategoryService = academyCategoryService;
            this._academyCategoryModelFactory = academyCategoryModelFactory;
            this._academyModelFactory = academyModelFactory;

            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Details(int academyCategoryid)
        {
            var academyCategory = _academyCategoryService.GetById(academyCategoryid, cache => default);
            if (academyCategory == null)
                return InvokeHttp404();

            if ((academyCategory.Deleted || !academyCategory.IsActive) && _workContext.CurrentUser.RoleGroup.RoleGroupType != Core.Domain.Roles.RoleGroupType.WCore)
                return NotFound();

            var model = academyCategory.ToModel<AcademyCategoryModel>();
            _academyCategoryModelFactory.PrepareAcademyCategoryModel(model, academyCategory);

            model.PageTitle = new Models.PageTitleModel()
            {
                Title = model.Title,
                Image = model.Image
            };


            var academyPaging = new AcademyPagingFilteringModel()
            {
                AcademyCategoryId = academyCategoryid
            };
            model.Academies = _academyModelFactory.PrepareAcademyListModel(academyPaging);

            return View(model);
        }
        #endregion
    }
}

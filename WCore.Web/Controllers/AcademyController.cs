
using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Academies;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Academies;
using WCore.Web.Models.ViewModels;

namespace WCore.Web.Controllers
{
    public class AcademyController : BasePublicController
    {
        #region Fields
        private readonly IAcademyCategoryModelFactory _academyCategoryModelFactory;
        private readonly IAcademyModelFactory _academyModelFactory;
        private readonly IAcademyFileModelFactory _academyFileModelFactory;
        private readonly IAcademyImageModelFactory _academyImageModelFactory;
        private readonly IAcademyVideoModelFactory _academyVideoModelFactory;
        private readonly IAcademyService _academyService;


        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public AcademyController(IAcademyModelFactory academyModelFactory,
            IAcademyCategoryModelFactory academyCategoryModelFactory,
            IAcademyFileModelFactory academyFileModelFactory,
            IAcademyImageModelFactory academyImageModelFactory,
            IAcademyVideoModelFactory academyVideoModelFactory,
            IAcademyService academyService,
            IUserModelFactory userModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._academyModelFactory = academyModelFactory;
            this._academyCategoryModelFactory = academyCategoryModelFactory;
            this._academyFileModelFactory = academyFileModelFactory;
            this._academyImageModelFactory = academyImageModelFactory;
            this._academyVideoModelFactory = academyVideoModelFactory;
            this._academyService = academyService;

            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Details(int academyid)
        {
            var academy = _academyService.GetById(academyid, cache => default);
            if (academy == null)
                return InvokeHttp404();

            if ((academy.Deleted || !academy.IsActive) && _workContext.CurrentUser.RoleGroup.RoleGroupType != Core.Domain.Roles.RoleGroupType.WCore)
                return NotFound();

            var model = new AcademyViewModel();

            if (academy != null)
            {
                model.Academy = academy.ToModel<AcademyModel>();
                _academyModelFactory.PrepareAcademyModel(model.Academy, academy);

                model.Academy.PageTitle = new Models.PageTitleModel()
                {
                    Title = model.Academy.Title,
                    Image = model.Academy.Image
                };
                model.Academy.AcademyImages = _academyImageModelFactory.PrepareAcademyImageListModel(new AcademyImagePagingFilteringModel() { AcademyId = academyid });
                model.Academy.AcademyFiles = _academyFileModelFactory.PrepareAcademyFileListModel(new AcademyFilePagingFilteringModel() { AcademyId = academyid });
                model.Academy.AcademyVideos = _academyVideoModelFactory.PrepareAcademyVideoListModel(new AcademyVideoPagingFilteringModel() { AcademyId = academyid });

                model.AcademyCategoryList = _academyCategoryModelFactory.PrepareAcademyCategoryListModel(new AcademyCategoryPagingFilteringModel() { });
            }
            return View(model);
        }
        #endregion
    }
}

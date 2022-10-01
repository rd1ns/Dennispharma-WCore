
using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Congresses;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Congresses;

namespace WCore.Web.Controllers
{
    public class CongressController : BasePublicController
    {
        #region Fields
        private readonly ICongressModelFactory _congressModelFactory;
        private readonly ICongressService _congressService;
        private readonly ICongressImageModelFactory _congressImageModelFactory;


        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public CongressController(ICongressModelFactory congressModelFactory,
            ICongressService congressService,
            IUserModelFactory userModelFactory,
            ICongressImageModelFactory congressImageModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._congressModelFactory = congressModelFactory;
            this._congressService = congressService;
            this._congressImageModelFactory= congressImageModelFactory;

            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Details(int congressid)
        {
            var congress = _congressService.GetById(congressid, cache => default);
            if (congress == null)
                return InvokeHttp404();

            if ((congress.Deleted || !congress.IsActive) && _workContext.CurrentUser.RoleGroup.RoleGroupType != Core.Domain.Roles.RoleGroupType.WCore)
                return NotFound();

            var model = new CongressModel();
            if (congress != null)
            {
                model = congress.ToModel<CongressModel>();
                _congressModelFactory.PrepareCongressModel(model, congress);

                model.PageTitle = new Models.PageTitleModel()
                {
                    Title = model.Title,
                    Image = model.Image
                };

                model.CongressImages = _congressImageModelFactory.PrepareCongressImageListModel(new CongressImagePagingFilteringModel() { CongressId = congressid });
            }
            return View(model);
        }
        #endregion
    }
}

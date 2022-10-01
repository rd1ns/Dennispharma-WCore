using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Teams;
using WCore.Core.Infrastructure;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Services.Teams;
using WCore.Web.Areas.Admin.Helpers;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Teams;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class TeamController : BaseAdminController
    {
        #region Fields
        private readonly ITeamService _teamService;
        private readonly ITeamCategoryService _teamCategoryService;

        private readonly IWCoreFileProvider _wCoreFileProvider;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        private readonly ImageHelper _imageHelper;
        #endregion

        #region Ctor
        public TeamController(ITeamService teamService,
            ITeamCategoryService teamCategoryService,
            IWCoreFileProvider wCoreFileProvider,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ILocalizedModelFactory localizedModelFactory,
            IUrlRecordService urlRecordService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._teamService = teamService;
            this._teamCategoryService = teamCategoryService;

            this._wCoreFileProvider = wCoreFileProvider;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
            this._urlRecordService = urlRecordService;
            this._webHostEnvironment = webHostEnvironment;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._workContext = workContext;

            _imageHelper = new ImageHelper();

        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/Team/List");
        }

        public IActionResult List()
        {
            var searchModel = new TeamSearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(TeamSearchModel searchModel)
        {
            var team = _teamService.GetAllByFilters(
                searchModel.TeamCategoryId,
                searchModel.Name,
                searchModel.Surname,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.ShowOnHome,
                searchModel.skip,
                searchModel.take);

            var model = new TeamListModel().PrepareToGrid(searchModel, team, () =>
            {
                return team.Select(team =>
                {
                    var m = team.ToModel<TeamModel>();
                    m.TeamCategory = _teamCategoryService.GetById(team.TeamCategoryId).ToModel<TeamCategoryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _teamService.GetById(Id);

            if (entity == null)
                entity = new Team();

            var model = entity.ToModel<TeamModel>();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(TeamModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<Team>();

            #region Delete
            if (delete)
            {
                var _entity = _teamService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _teamService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/team");

            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                    await file.CopyToAsync(fileStream);
                    entity.Image = "/uploads/team/" + file.FileName;
                }
            }

            if (!Request.Form.Files.Any() && entity.Id != 0)
            {
                var u = _teamService.GetById(entity.Id);
                entity.Image = u.Image;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _teamService.Insert(entity);
            }
            else
            {
                _teamService.Update(entity);
            }
            #endregion

            return Json(continueEditing);
        }
        #endregion
    }
}

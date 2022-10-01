using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Teams;
using WCore.Core.Infrastructure;
using WCore.Framework.Extensions;
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
    public class TeamCategoryController : BaseAdminController
    {
        #region Fields
        private readonly ITeamCategoryService _teamCategoryService;
        private readonly ITeamService _teamService;

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
        public TeamCategoryController(ITeamCategoryService teamCategoryService,
            ITeamService teamService,
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
            this._teamCategoryService = teamCategoryService;
            this._teamService = teamService;

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

        #region Utilities
        protected virtual void UpdateLocales(TeamCategory entity, TeamCategoryModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);


                //search engine name
                var seName = _urlRecordService.ValidateSeName(entity, localized.SeName, localized.Title, false);
                _urlRecordService.SaveSlug(entity, seName, localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/TeamCategory/List");
        }

        public IActionResult List()
        {
            var searchModel = new TeamCategorySearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(TeamCategorySearchModel searchModel)
        {
            var teamCategory = _teamCategoryService.GetAllByFilters(
                searchModel.ParentId,
                searchModel.Title,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new TeamCategoryListModel().PrepareToGrid(searchModel, teamCategory, () =>
            {
                return teamCategory.Select(teamCategory =>
                {
                    var m = teamCategory.ToModel<TeamCategoryModel>();
                    m.Parent = _teamCategoryService.GetById(teamCategory.ParentId).ToModel<TeamCategoryModel>();
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _teamCategoryService.GetById(Id);

            if (entity == null)
                entity = new TeamCategory();

            var model = entity.ToModel<TeamCategoryModel>();

            Action<TeamCategoryLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                model.SeName = _urlRecordService.GetSeName(entity, 0, true, false);
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(entity, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            model.Parents = new SelectList(_teamCategoryService.GetAllByFilters(), "Id", "Title", model.ParentId).InsertEmptyFirst(_localizationService.GetResource("admin.configuration.root"), "0").ToList();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult AddOrEdit(TeamCategoryModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<TeamCategory>();

            #region Delete
            if (delete)
            {
                var _entity = _teamCategoryService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _teamCategoryService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _teamCategoryService.Insert(entity);
            }
            else
            {
                _teamCategoryService.Update(entity);
            }
            //search engine name
            model.SeName = _urlRecordService.ValidateSeName(entity, model.SeName, entity.Title, true);
            _urlRecordService.SaveSlug(entity, model.SeName, 0);

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(continueEditing);
        }
        #endregion
    }
}

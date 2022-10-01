using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Popup;
using WCore.Framework.Extensions;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Common;
using WCore.Services.Localization;
using WCore.Services.Popups;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Popups;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class PopupController : BaseAdminController
    {
        #region Fields
        private readonly IPopupService _popupService;

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ICurrencyService _currencyService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public PopupController(IPopupService popupService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ILocalizedModelFactory localizedModelFactory,
            IUrlRecordService urlRecordService,
            IWebHostEnvironment webHostEnvironment,

            ICurrencyService currencyService,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._popupService = popupService;

            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
            this._urlRecordService = urlRecordService;
            this._webHostEnvironment = webHostEnvironment;

            this._currencyService = currencyService;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._workContext = workContext;

        }
        #endregion

        #region Utilities

        protected virtual void UpdateLocales(Popup entity, PopupModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Body,
                    localized.Body,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/Popup/List");
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetFilteredItems(PopupSearchModel searchModel)
        {
            var popups = _popupService.GetAllByFilters(searchModel.ShowUrl,
                searchModel.ShowOn,
                searchModel.skip,
                searchModel.take);

            var model = new PopupListModel().PrepareToGrid(searchModel, popups, () =>
            {
                return popups.Select(popup =>
                {
                    var m = popup.ToModel<PopupModel>();
                    m.PopupShowTypeName = popup.PopupShowType.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    m.PopupTimeTypeName = popup.PopupTimeType.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id, int? popupCategoryId = null)
        {
            var entity = _popupService.GetById(Id);

            if (entity == null)
                entity = new Popup();

            var model = entity.ToModel<PopupModel>();

            Action<PopupLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                };
            }

            model.PopupShowTypes = model.PopupShowType.ToSelectList().InsertEmptyFirst(_localizationService.GetResource("admin.configuration.all")).ToList();
            model.PopupTimeTypes = model.PopupTimeType.ToSelectList().InsertEmptyFirst(_localizationService.GetResource("admin.configuration.all")).ToList();
            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public IActionResult AddOrEdit(PopupModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<Popup>();

            #region Delete
            if (delete)
            {
                var _entity = _popupService.GetById(model.Id);
                _popupService.Delete(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Add Or Update
            if (model.Id == 0)
            {
                _popupService.Insert(entity);
            }
            else
            {
                _popupService.Update(entity);
            }

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(continueEditing);
        }
        #endregion
    }
}

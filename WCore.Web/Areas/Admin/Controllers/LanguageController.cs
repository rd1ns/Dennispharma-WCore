using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Core.Domain.Localization;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Framework.Mvc;
using WCore.Framework.Mvc.ModelBinding;
using WCore.Framework.Validators;
using WCore.Services.Common;
using WCore.Services.Localization;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Localization;
using System;
using System.Linq;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class LanguageController : BaseAdminController
    {
        #region Fields
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrencyService _currencyService;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor
        public LanguageController(ILanguageService languageService,
            ILocalizationService localizationService,
            ICurrencyService currencyService,
            IWebHostEnvironment webHostEnvironment,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._currencyService = currencyService;

            this._webHostEnvironment = webHostEnvironment;
            this._settingService = settingService;
            this._webHelper = webHelper;

        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/Language/List");
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetFilteredItems()
        {
            var _skip = int.Parse(Request.Form["skip"]);
            var _take = int.Parse(Request.Form["take"]);
            var _query = Request.Form["filter[filters][0][value]"].ToString();

            var languages = _languageService.GetAllByFilters(searchValue: _query, skip: _skip, take: _take);
            var searchModel = new LanguageSearchModel();

            //prepare list model
            var model = new LanguageListModel().PrepareToGrid(searchModel, languages, () =>
            {
                return languages.Select(language =>
                {
                    //fill in model values from the entity
                    var languageModel = language.ToModel<LanguageModel>();

                    return languageModel;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _languageService.GetById(Id).ToModel<LanguageModel>();

            if (entity == null)
                entity = new LanguageModel();

            entity.Currencies = _currencyService.GetAllByFilters(take: int.MaxValue).Select(o =>
            {
                var m = new SelectListItem();
                m.Value = o.Id.ToString();
                m.Text = o.Name;
                m.Selected = entity.Id == o.Id;
                return m;
            }).ToList();

            return View(entity);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public IActionResult AddOrEdit(LanguageModel language, bool continueEditing, bool sendInfo, bool delete)
        {
            var entity = language.ToEntity<Language>();

            if (delete)
            {
                var _entity = _languageService.GetById(language.Id);
                _entity.Published = true;
                _languageService.Update(_entity);
                return Json("Deleted");
            }


            if (language.Id == 0)
            {
                _languageService.Insert(entity);
            }
            else
            {
                _languageService.Update(entity);
            }

            return Json(continueEditing);
        }
        #endregion

        #region Resources

        [HttpPost]
        public virtual IActionResult Resources(LocaleResourceSearchModel searchModel)
        {
            //try to get a language with the specified id
            var language = _languageService.GetById(searchModel.LanguageId);
            if (language == null)
                return RedirectToAction("List");

            //prepare model
            var languages = _localizationService.GetAllByFilters(searchModel.LanguageId, searchModel.SearchResourceValue, searchModel.SearchResourceName, skip: searchModel.skip, take: searchModel.take);


            var model = new LocaleResourceListModel().PrepareToGrid(searchModel, languages, () =>
            {
                //fill in model values from the entity
                return languages.Select(localeResource => new LocaleResourceModel
                {
                    LanguageId = language.Id,
                    Id = localeResource.Id,
                    ResourceName = localeResource.ResourceName,
                    ResourceValue = localeResource.ResourceValue
                });
            });

            return Json(model);
        }

        //ValidateAttribute is used to force model validation
        [HttpPost]
        public virtual IActionResult ResourceUpdate([Validate] LocaleResourceModel model)
        {

            if (model.ResourceName != null)
                model.ResourceName = model.ResourceName.Trim();
            if (model.ResourceValue != null)
                model.ResourceValue = model.ResourceValue.Trim();

            if (!ModelState.IsValid)
            {
                return ErrorJson(ModelState.SerializeErrors());
            }

            var resource = _localizationService.GetById(model.Id);
            // if the resourceName changed, ensure it isn't being used by another resource
            if (!resource.ResourceName.Equals(model.ResourceName, StringComparison.InvariantCultureIgnoreCase))
            {
                var res = _localizationService.GetLocaleStringResourceByName(model.ResourceName, model.LanguageId, false);
                if (res != null && res.Id != resource.Id)
                {
                    return ErrorJson(string.Format(_localizationService.GetResource("Admin.Configuration.Languages.Resources.NameAlreadyExists"), res.ResourceName));
                }
            }

            //fill entity from model
            resource = model.ToEntity(resource);

            _localizationService.UpdateLocaleStringResource(resource);

            return new NullJsonResult();
        }

        //ValidateAttribute is used to force model validation
        [HttpPost]
        public virtual IActionResult ResourceAdd(int languageId, [Validate] LocaleResourceModel model)
        {
            if (model.ResourceName != null)
                model.ResourceName = model.ResourceName.Trim();
            if (model.ResourceValue != null)
                model.ResourceValue = model.ResourceValue.Trim();

            if (!ModelState.IsValid)
            {
                return ErrorJson(ModelState.SerializeErrors());
            }

            var res = _localizationService.GetLocaleStringResourceByName(model.ResourceName, model.LanguageId, false);
            if (res == null)
            {
                //fill entity from model
                var resource = model.ToEntity<LocaleStringResource>();

                resource.LanguageId = languageId;

                _localizationService.InsertLocaleStringResource(resource);
            }
            else
            {
                return ErrorJson(string.Format(_localizationService.GetResource("Admin.Configuration.Languages.Resources.NameAlreadyExists"), model.ResourceName));
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ResourceDelete(int id)
        {
            //try to get a locale resource with the specified id
            var resource = _localizationService.GetById(id)
                ?? throw new ArgumentException("No resource found with the specified id", nameof(id));

            _localizationService.Delete(resource);

            return new NullJsonResult();
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Infrastructure;
using WCore.Framework.Extensions;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.DynamicForms;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Helpers;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.DynamicForms;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class DynamicFormController : BaseAdminController
    {
        #region Fields
        private readonly IDynamicFormService _dynamicFormService;
        private readonly IDynamicFormElementService _dynamicFormElementService;
        private readonly IDynamicFormRecordService _dynamicFormRecordService;
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
        public DynamicFormController(IDynamicFormService dynamicFormService,
            IDynamicFormElementService dynamicFormElementService,
            IDynamicFormRecordService dynamicFormRecordService,
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
            this._dynamicFormService = dynamicFormService;
            this._dynamicFormElementService = dynamicFormElementService;
            this._dynamicFormRecordService = dynamicFormRecordService;
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

        protected virtual void UpdateLocales(DynamicForm entity, DynamicFormModel model)
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

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Result,
                    localized.Result,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Methods
        public IActionResult Index()
        {
            return Redirect("/Admin/DynamicForm/List");
        }

        public IActionResult List()
        {
            var searchModel = new DynamicFormSearchModel();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(DynamicFormSearchModel searchModel)
        {
            var activities = _dynamicFormService.GetAllByFilters(searchModel.DynamicFormType, searchModel.ShowOn, searchModel.IsActive, searchModel.Deleted, searchModel.skip, searchModel.take);

            var model = new DynamicFormListModel().PrepareToGrid(searchModel, activities, () =>
            {
                return activities.Select(dynamicForm =>
                {
                    var m = dynamicForm.ToModel<DynamicFormModel>();
                    m.DynamicFormTypeName = m.DynamicFormType.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    return m;
                });
            });
            return Json(model);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _dynamicFormService.GetById(Id);

            if (entity == null)
                entity = new DynamicForm();

            var model = entity.ToModel<DynamicFormModel>();

            Action<DynamicFormLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                    locale.Result = _localizationService.GetLocalized(entity, e => e.Result, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            model.DynamicFormTypes = model.DynamicFormType.ToSelectList().ToList();

            if (model.Id != 0)
            {
                var currentElements = _dynamicFormElementService.GetAllByFilters(model.Id).Select(o =>
                {
                    var dm = o.ToModel<DynamicFormElementModel>();
                    dm.ControlElementName = dm.ControlElement.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    dm.ControlElements = dm.ControlElement.ToSelectList().ToList();
                    return dm;

                }).ToList();
                if (currentElements.Any())
                {

                    model.DynamicFormElements = currentElements;
                }
                else
                {
                    model.DynamicFormElements = new System.Collections.Generic.List<DynamicFormElementModel>()
                    {
                        new DynamicFormElementModel()
                        {
                            ControlElement = ControlElement.List,
                            ControlLabel = "",
                            DynamicFormId = model.Id,
                            ControlValue = "",
                            Required = false,
                            ControlElements = ControlElement.List.ToSelectList().ToList()
                        }
                    };
                }
            }
            else
            {
                model.DynamicFormElements = new System.Collections.Generic.List<DynamicFormElementModel>()
                {
                    new DynamicFormElementModel()
                    {
                        ControlElement = ControlElement.List,
                        ControlLabel = "",
                        DynamicFormId = 0,
                        ControlValue = "",
                        Required = false,
                        ControlElements = ControlElement.List.ToSelectList().ToList()
                    }
                };
            }

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async System.Threading.Tasks.Task<IActionResult> AddOrEdit(DynamicFormModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<DynamicForm>();

            #region Delete
            if (delete)
            {
                var _entity = _dynamicFormService.GetById(model.Id);
                _entity.Deleted = true;
                _entity.IsActive = false;
                _dynamicFormService.Update(_entity);
                return Json("Deleted");
            }
            #endregion

            #region Image
            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var imageModel = await _imageHelper.ConvertImage(file, false, false, "dynamicForm");
                    entity.Image = imageModel.Original;
                }
            }

            if (!Request.Form.Files.Any() && entity.Id != 0)
            {
                var u = _dynamicFormService.GetById(entity.Id);
                entity.Image = u.Image;
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _dynamicFormService.Insert(entity);
                foreach (var dynamicFormElement in model.DynamicFormElements)
                {
                    _dynamicFormElementService.Insert(dynamicFormElement.ToEntity<DynamicFormElement>());
                }
            }
            else
            {
                _dynamicFormService.Update(entity);
                foreach (var dynamicFormElement in model.DynamicFormElements)
                {
                    if (dynamicFormElement.Id == 0)
                    {
                        _dynamicFormElementService.Insert(dynamicFormElement.ToEntity<DynamicFormElement>());
                    }
                    else if(dynamicFormElement.ControlValue == "Delete" && dynamicFormElement.ControlLabel == "Delete")
                    {
                        _dynamicFormElementService.Delete(dynamicFormElement.Id);
                    }
                    else
                    {
                        _dynamicFormElementService.Update(dynamicFormElement.ToEntity<DynamicFormElement>());
                    }
                }
            }

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(entity);
        }
        #endregion

        #region DynamicForm Element Methods
        [HttpPost]
        public JsonResult GetDynamicFormElementFilteredItems(DynamicFormElementSearchModel searchModel)
        {
            var dynamicFormElements = _dynamicFormElementService.GetAllByFilters(searchModel.DynamicFormId);

            var model = new DynamicFormElementListModel().PrepareToGrid(searchModel, dynamicFormElements, () =>
            {
                return dynamicFormElements.Select(dynamicFormElement =>
                {
                    var m = dynamicFormElement.ToModel<DynamicFormElementModel>();
                    return m;
                });
            });
            return Json(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult AddOrEditDynamicFormElement(DynamicFormElementModel model, bool continueEditing, bool delete)
        {
            var entity = model.ToEntity<DynamicFormElement>();

            #region Delete
            if (delete)
            {
                var _entity = _dynamicFormElementService.GetById(model.Id);
                _dynamicFormElementService.Delete(_entity.Id);
                return Json("Deleted");
            }
            #endregion

            #region Add Or Update

            if (model.Id == 0)
            {
                _dynamicFormElementService.Insert(entity);
            }
            else
            {
                _dynamicFormElementService.Update(entity);
            }

            #endregion

            return Json(entity);
        }
        #endregion
    }
}

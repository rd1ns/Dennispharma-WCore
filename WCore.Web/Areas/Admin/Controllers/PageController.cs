using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WCore.Core;
using WCore.Core.Domain.Pages;
using WCore.Framework.Extensions;
using WCore.Framework.Factories;
using WCore.Framework.Filters;
using WCore.Framework.Models;
using WCore.Services.Localization;
using WCore.Services.Pages;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Pages;
using System;
using System.IO;
using System.Linq;
using WCore.Web.Areas.Admin.Helpers;
using WCore.Services.Galleries;
using WCore.Services.DynamicForms;
using WCore.Web.Areas.Admin.Models.DynamicForms;
using WCore.Web.Areas.Admin.Models.Galleries;
using System.Collections.Generic;

namespace WCore.Web.Areas.Admin.Controllers
{
    public class PageController : BaseAdminController
    {
        #region Fields
        private readonly IPageService _pageService;
        private readonly IGalleryService _galleryService;
        private readonly IDynamicFormService _dynamicFormService;

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
        public PageController(IPageService pageService,
            IGalleryService galleryService,
            IDynamicFormService dynamicFormService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            ILocalizedModelFactory localizedModelFactory,
            IUrlRecordService urlRecordService,
            IWebHostEnvironment webHostEnvironment,

            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._pageService = pageService;
            this._galleryService = galleryService;
            this._dynamicFormService = dynamicFormService;

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

        protected virtual void UpdateLocales(Page entity, PageModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Title,
                    localized.Title,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.ShortBody,
                    localized.ShortBody,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Body,
                    localized.Body,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.LeftBody,
                    localized.LeftBody,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.RightBody,
                    localized.RightBody,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.RedirectPageUrl,
                    localized.RedirectPageUrl,
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
            return Redirect("/Admin/Page/List");
        }

        public IActionResult List()
        {
            var model = new PageModel();
            var searchModel = new PageSearchModel();
            searchModel.PageTypes = model.PageType.ToSelectList().InsertEmptyFirst(_localizationService.GetResource("admin.configuration.all")).ToList();
            searchModel.FooterLocations = model.FooterLocation.ToSelectList().InsertEmptyFirst(_localizationService.GetResource("admin.configuration.all")).ToList();
            return View(searchModel);
        }

        [HttpPost]
        public JsonResult GetFilteredItems(PageSearchModel searchModel)
        {

            searchModel.take = (searchModel.take == 0 ? 100 : searchModel.take);

            var pages = _pageService.GetAllByFilters(searchModel.Query,
                searchModel.PageType,
                searchModel.FooterLocation,
                searchModel.ParentId,
                searchModel.IsActive,
                searchModel.Deleted,
                searchModel.ShowOn,
                searchModel.HomePage,
                searchModel.RedirectPage,
                searchModel.skip,
                searchModel.take).ToList().Select(page =>
                {
                    var m = page.ToModel<PageModel>();
                    m.PageTypeName = page.PageType.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    m.FooterLocationName = page.FooterLocation.GetLocalizedEnum(_localizationService, _workContext.WorkingLanguage.Id);
                    m.DynamicForm = m.DynamicFormId != 0 ? _dynamicFormService.GetById(m.DynamicFormId).ToModel<DynamicFormModel>() : null;
                    m.Gallery = m.GalleryId != 0 ? _galleryService.GetById(m.GalleryId).ToModel<GalleryModel>() : null;
                    return m;
                }).ToList();

            return Json(pages);
        }

        public IActionResult AddOrEdit(int Id)
        {
            var entity = _pageService.GetById(Id);

            if (entity == null)
                entity = new Page();


            var model = entity.ToModel<PageModel>();

            Action<PageLocalizedModel, int> localizedModelConfiguration = null;

            if (model.Id != 0)
            {
                model.SeName = _urlRecordService.GetSeName(entity, 0, true, false);
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                    locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                    locale.ShortBody = _localizationService.GetLocalized(entity, e => e.ShortBody, languageId, false, false);
                    locale.MetaTitle = _localizationService.GetLocalized(entity, e => e.MetaTitle, languageId, false, false);
                    locale.MetaKeywords = _localizationService.GetLocalized(entity, e => e.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = _localizationService.GetLocalized(entity, e => entity.MetaDescription, languageId, false, false);
                    locale.RedirectPageUrl = _localizationService.GetLocalized(entity, e => entity.RedirectPageUrl, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(entity, languageId, false, false);
                };
            }

            model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            model.Galleries = new SelectList(_galleryService.GetAllByFilters(), "Id", "Title", model.GalleryId).InsertEmptyFirst(_localizationService.GetResource("admin.configuration.notselected"), "0").ToList();
            model.DynamicForms = new SelectList(_dynamicFormService.GetAllByFilters(), "Id", "Title", model.DynamicFormId).InsertEmptyFirst(_localizationService.GetResource("admin.configuration.notselected"), "0").ToList();

            model.PageTypes = model.PageType.ToSelectList().ToList();
            model.FooterLocations = model.FooterLocation.ToSelectList().ToList();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [HttpPost, ParameterBasedOnFormName("send-info", "sendInfo")]
        public async System.Threading.Tasks.Task<IActionResult> AddOrEditAsync(PageModel model, bool continueEditing, bool sendInfo, bool delete)
        {
            var entity = model.ToEntity<Page>();

            #region Delete
            if (delete)
            {
                _pageService.Delete(entity.Id);
                return Json("Deleted");
            }
            #endregion

            #region Image
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/skischool");

            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    var imageModel = await _imageHelper.ConvertImage(file, false, false, "page");
                    entity.Image = imageModel.Original;
                }
            }

            if (!Request.Form.Files.Any() && entity.Id != 0)
            {
                var u = _pageService.GetById(entity.Id);
                entity.Image = u.Image;
            }
            #endregion

            #region Add Or Update
            if (model.Id == 0)
            {
                _pageService.Insert(entity);
            }
            else
            {
                _pageService.Update(entity);
            }

            //search engine name
            model.SeName = _urlRecordService.ValidateSeName(entity, model.SeName, entity.Title, true);
            _urlRecordService.SaveSlug(entity, model.SeName, 0);

            //locales
            UpdateLocales(entity, model);
            #endregion

            return Json(continueEditing);
        }

        public IActionResult Copy(int Id,  string titles)
        {
            var entity = _pageService.GetById(Id);
            var model = entity.ToModel<PageModel>();

            Action<PageLocalizedModel, int> localizedModelConfiguration = null;

            localizedModelConfiguration = (locale, languageId) =>
            {
                locale.Title = _localizationService.GetLocalized(entity, e => e.Title, languageId, false, false);
                locale.Body = _localizationService.GetLocalized(entity, e => e.Body, languageId, false, false);
                locale.ShortBody = _localizationService.GetLocalized(entity, e => e.ShortBody, languageId, false, false);
                locale.MetaTitle = _localizationService.GetLocalized(entity, e => e.MetaTitle, languageId, false, false);
                locale.MetaKeywords = _localizationService.GetLocalized(entity, e => e.MetaKeywords, languageId, false, false);
                locale.MetaDescription = _localizationService.GetLocalized(entity, e => entity.MetaDescription, languageId, false, false);
                locale.RedirectPageUrl = _localizationService.GetLocalized(entity, e => entity.RedirectPageUrl, languageId, false, false);
                locale.SeName = _urlRecordService.GetSeName(entity, languageId, false, false);
            };
            var entityLocales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            var _entityLocals = new List<PageLocalizedModel>();

            var titleList = titles.Split(",");

            var entityLocalesIndex = 1;
            entityLocales = entityLocales.Select(o =>
            {
                var entityLocal = o;
                if (titleList.Length >= 2)
                {
                    if (titleList[entityLocalesIndex] != null)
                    {
                        entityLocal.Title = titleList[entityLocalesIndex];
                    }
                    entityLocalesIndex++;
                }
                return entityLocal;

            }).ToList();
            model.Locales = entityLocales;
            entity.Title = titleList[0];
            entity.Id = 0;
            _pageService.Insert(entity);

            //locales
            UpdateLocales(entity, model);

            return Json("COPIED");
        }

        public List<PageModel> PagesWithBreadcrumb(List<PageModel> _pageList = null,
            int? ParentId = null,
            bool? ShowOn = null,
            bool? IsSub = false,
            string Title = "")
        {
            if (_pageList == null && IsSub == false)
            {
                _pageList = new List<PageModel>();
            }

            var pages = _pageService.GetAllByFilters(ParentId: ParentId).ToList();

            foreach (var page in pages)
            {
                var p = page.ToModel<PageModel>();
                p.Title = Title + page.Title;
                _pageList.Add(p);
                var subContent = _pageService.GetAllByFilters(ParentId: page.Id, ShowOn: ShowOn).ToList();
                if (subContent.Count > 0)
                {
                    PagesWithBreadcrumb(_pageList, ParentId: page.Id, Title: Title + page.Title + " > ", IsSub: true);
                }
            }

            return _pageList;
        }
        #endregion
    }
}

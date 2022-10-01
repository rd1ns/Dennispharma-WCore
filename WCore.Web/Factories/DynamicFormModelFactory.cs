using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Caching.Extensions;
using WCore.Services.Common;
using WCore.Services.DynamicForms;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.DynamicForms;

namespace WCore.Web.Factories
{
    public interface IDynamicFormModelFactory
    {
        void PrepareDynamicFormModel(DynamicFormModel model, DynamicForm entity);
        DynamicFormModel PrepareDynamicFormModel(DynamicFormType DynamicFormType);
        DynamicFormModel PrepareDynamicFormModel(int DynamicFormId);
        DynamicFormModel PrepareDynamicFormModel(DynamicForm entity);
        DynamicFormListModel PrepareDynamicFormListModel(DynamicFormPagingFilteringModel command);
    }

    public class DynamicFormModelFactory : IDynamicFormModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IDynamicFormService _dynamicFormService;
        private readonly IDynamicFormElementService _dynamicFormElementService;
        private readonly IDynamicFormElementModelFactory _dynamicFormElementModelFactory;

        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        #endregion

        #region Methods
        public DynamicFormModelFactory(UserSettings userSettings,
        IDynamicFormService dynamicFormService,
        IDynamicFormElementService dynamicFormElementService,
        IDynamicFormElementModelFactory dynamicFormElementModelFactory,
        ILocalizationService localizationService,
        IUserService userService,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        IStaticCacheManager staticCacheManager,
        IUrlRecordService urlRecordService,
        IWorkContext workContext,
        MediaSettings mediaSettings)
        {
            this._userSettings = userSettings;
            this._dynamicFormService = dynamicFormService;
            this._dynamicFormElementService = dynamicFormElementService;
            this._dynamicFormElementModelFactory = dynamicFormElementModelFactory;


            this._localizationService = localizationService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._genericAttributeService = genericAttributeService;
            this._staticCacheManager = staticCacheManager;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
        }
        #endregion
        public virtual DynamicFormModel PrepareDynamicFormModel(DynamicFormType DynamicFormType)
        {
            var entity = _dynamicFormService.GetByDynamicFormType(DynamicFormType);

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<DynamicFormModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);
            model.Result = _localizationService.GetLocalized(entity, x => x.Result);

            model.DynamicFormElements = _dynamicFormElementService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<DynamicFormElementModel>();
                _dynamicFormElementModelFactory.PrepareDynamicFormElementModel(gi, o);
                return gi;
            }).ToList();

            return model;
        }
        public virtual DynamicFormModel PrepareDynamicFormModel(int DynamicFormId)
        {
            var entity = _dynamicFormService.GetById(DynamicFormId);

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<DynamicFormModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);
            model.Result = _localizationService.GetLocalized(entity, x => x.Result);

            model.DynamicFormElements = _dynamicFormElementService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<DynamicFormElementModel>();
                _dynamicFormElementModelFactory.PrepareDynamicFormElementModel(gi, o);
                return gi;
            }).ToList();

            return model;
        }
        public virtual DynamicFormModel PrepareDynamicFormModel(DynamicForm entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<DynamicFormModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);
            model.Result = _localizationService.GetLocalized(entity, x => x.Result);

            model.DynamicFormElements = _dynamicFormElementService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<DynamicFormElementModel>();
                _dynamicFormElementModelFactory.PrepareDynamicFormElementModel(gi, o);
                return gi;
            }).ToList();

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">DynamicForm post model</param>
        /// <param name="activity">DynamicForm post entity</param>
        /// <param name="prepareComments">Whether to prepare DynamicForm comments</param>
        public virtual void PrepareDynamicFormModel(DynamicFormModel model, DynamicForm entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);
            model.Result = _localizationService.GetLocalized(entity, x => x.Result);

            model.DynamicFormElements = _dynamicFormElementService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<DynamicFormElementModel>();
                _dynamicFormElementModelFactory.PrepareDynamicFormElementModel(gi, o);
                return gi;
            }).ToList();
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public DynamicFormListModel PrepareDynamicFormListModel(DynamicFormPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new DynamicFormListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<DynamicForm> dynamicForms = _dynamicFormService.GetAllByFilters(command.DynamicFormType, command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(dynamicForms);

            model.DynamicForms = dynamicForms
                .Select(x =>
                {
                    var entityModel = x.ToModel<DynamicFormModel>();
                    PrepareDynamicFormModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

namespace WCore.Web.Factories
{
    public interface IDynamicFormElementModelFactory
    {
        void PrepareDynamicFormElementModel(DynamicFormElementModel model, DynamicFormElement entity);
        DynamicFormElementModel PrepareDynamicFormElementModel(int DynamicFormElementId);
        DynamicFormElementModel PrepareDynamicFormElementModel(DynamicFormElement entity);
        DynamicFormElementListModel PrepareDynamicFormElementListModel(DynamicFormElementPagingFilteringModel command);
    }

    public class DynamicFormElementModelFactory : IDynamicFormElementModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IDynamicFormElementService _dynamicFormElementService;
        private readonly IDynamicFormService _dynamicFormService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        #endregion

        #region Methods
        public DynamicFormElementModelFactory(UserSettings userSettings,
        IDynamicFormElementService dynamicFormElementService,
        IDynamicFormService dynamicFormService,
        ILocalizationService localizationService,
        IUserService userService,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        IStaticCacheManager staticCacheManager,
        IUrlRecordService urlRecordService,
        IWorkContext workContext,
        MediaSettings mediaSettings)
        {
            this._userSettings = userSettings;
            this._dynamicFormElementService = dynamicFormElementService;
            this._dynamicFormService = dynamicFormService;


            this._localizationService = localizationService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._genericAttributeService = genericAttributeService;
            this._staticCacheManager = staticCacheManager;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
        }
        #endregion
        public virtual DynamicFormElementModel PrepareDynamicFormElementModel(int DynamicFormElementId)
        {
            var entity = _dynamicFormElementService.GetById(DynamicFormElementId);

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<DynamicFormElementModel>();

            var dynamicForm = _dynamicFormService.ToCachedGetById(model.DynamicFormId);

            if (dynamicForm != null && dynamicForm.UseLocalization)
            {
                model.ControlLabel = _localizationService.GetResource("common.df.label." + model.DynamicFormId + "." + model.Id);
                model.ControlValue = _localizationService.GetResource("common.df.value." + model.DynamicFormId + "." + model.Id);
            }

            return model;
        }
        public virtual DynamicFormElementModel PrepareDynamicFormElementModel(DynamicFormElement entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<DynamicFormElementModel>();

            var dynamicForm = _dynamicFormService.ToCachedGetById(model.DynamicFormId);

            if (dynamicForm != null && dynamicForm.UseLocalization)
            {
                model.ControlLabel = _localizationService.GetResource("common.df.label." + model.DynamicFormId + "." + model.Id);
                model.ControlValue = _localizationService.GetResource("common.df.value." + model.DynamicFormId + "." + model.Id);
            }

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">DynamicFormElement post model</param>
        /// <param name="activity">DynamicFormElement post entity</param>
        /// <param name="prepareComments">Whether to prepare DynamicFormElement comments</param>
        public virtual void PrepareDynamicFormElementModel(DynamicFormElementModel model, DynamicFormElement entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var dynamicForm = _dynamicFormService.ToCachedGetById(model.DynamicFormId);

            if (dynamicForm != null && dynamicForm.UseLocalization)
            {
                model.ControlLabel = _localizationService.GetResource("common.df.label." + model.DynamicFormId + "." + model.Id);
                model.ControlValue = _localizationService.GetResource("common.df.value." + model.DynamicFormId + "." + model.Id);
            }
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public DynamicFormElementListModel PrepareDynamicFormElementListModel(DynamicFormElementPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new DynamicFormElementListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<DynamicFormElement> dynamicFormElements = _dynamicFormElementService.GetAllByFilters(command.DynamicFormId);


            model.PagingFilteringContext.LoadPagedList(dynamicFormElements);

            model.DynamicFormElements = dynamicFormElements
                .Select(x =>
                {
                    var entityModel = x.ToModel<DynamicFormElementModel>();
                    PrepareDynamicFormElementModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}
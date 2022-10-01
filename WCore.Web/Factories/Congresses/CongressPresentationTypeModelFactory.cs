using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Congresses;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Common;
using WCore.Services.Congresses;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Congresses;

namespace WCore.Web.Factories
{
    public interface ICongressPresentationTypeModelFactory
    {
        void PrepareCongressPresentationTypeModel(CongressPresentationTypeModel model, CongressPresentationType entity);
        CongressPresentationTypeModel PrepareCongressPresentationTypeModel(CongressPresentationType entity);
        CongressPresentationTypeListModel PrepareCongressPresentationTypeListModel(CongressPresentationTypePagingFilteringModel command);
    }

    public class CongressPresentationTypeModelFactory : ICongressPresentationTypeModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ICongressPresentationTypeService _congressPresentationTypeService;

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
        public CongressPresentationTypeModelFactory(UserSettings userSettings,
        ICongressPresentationTypeService congressPresentationTypeService,
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
            this._congressPresentationTypeService = congressPresentationTypeService;

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
        public virtual CongressPresentationTypeModel PrepareCongressPresentationTypeModel(CongressPresentationType entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<CongressPresentationTypeModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">CongressPresentationType post model</param>
        /// <param name="congressPresentationType">CongressPresentationType post entity</param>
        /// <param name="prepareComments">Whether to prepare CongressPresentationType comments</param>
        public virtual void PrepareCongressPresentationTypeModel(CongressPresentationTypeModel model, CongressPresentationType entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public CongressPresentationTypeListModel PrepareCongressPresentationTypeListModel(CongressPresentationTypePagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new CongressPresentationTypeListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var congressPresentationTypes = _congressPresentationTypeService.GetAllByFilters(command.CongressId, command.Title, command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(congressPresentationTypes);

            model.CongressPresentationTypes = congressPresentationTypes
                .Select(x =>
                {
                    var entityModel = x.ToModel<CongressPresentationTypeModel>();
                    PrepareCongressPresentationTypeModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

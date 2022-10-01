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
    public interface ICongressPresentationModelFactory
    {
        void PrepareCongressPresentationModel(CongressPresentationModel model, CongressPresentation entity);
        CongressPresentationModel PrepareCongressPresentationModel(CongressPresentation entity);
        CongressPresentationListModel PrepareCongressPresentationListModel(CongressPresentationPagingFilteringModel command);
    }

    public class CongressPresentationModelFactory : ICongressPresentationModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ICongressPresentationService _congressPresentationService;

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
        public CongressPresentationModelFactory(UserSettings userSettings,
        ICongressPresentationService congressPresentationService,
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
            this._congressPresentationService = congressPresentationService;

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
        public virtual CongressPresentationModel PrepareCongressPresentationModel(CongressPresentation entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<CongressPresentationModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">CongressPresentation post model</param>
        /// <param name="congressPresentation">CongressPresentation post entity</param>
        /// <param name="prepareComments">Whether to prepare CongressPresentation comments</param>
        public virtual void PrepareCongressPresentationModel(CongressPresentationModel model, CongressPresentation entity)
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
        public CongressPresentationListModel PrepareCongressPresentationListModel(CongressPresentationPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new CongressPresentationListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var congressPresentations = _congressPresentationService.GetAllByFilters(command.CongressPresentationTypeId, command.CongressId, command.Title, "", "", "", command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(congressPresentations);

            model.CongressPresentations = congressPresentations
                .Select(x =>
                {
                    var entityModel = x.ToModel<CongressPresentationModel>();
                    PrepareCongressPresentationModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

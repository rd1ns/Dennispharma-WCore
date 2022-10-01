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
    public interface ICongressModelFactory
    {
        void PrepareCongressModel(CongressModel model, Congress entity);
        CongressModel PrepareCongressModel(Congress entity);
        CongressListModel PrepareCongressListModel(CongressPagingFilteringModel command);
    }

    public class CongressModelFactory : ICongressModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ICongressService _congressService;
        private readonly ICongressPaperTypeModelFactory _congressPaperTypeModelFactory;
        private readonly ICongressPresentationTypeModelFactory _congressPresentationTypeModelFactory;

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
        public CongressModelFactory(UserSettings userSettings,
        ICongressService congressService,
        ICongressPaperTypeModelFactory congressPaperTypeModelFactory,
        ICongressPresentationTypeModelFactory congressPresentationTypeModelFactory,
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
            this._congressService = congressService;
            this._congressPaperTypeModelFactory = congressPaperTypeModelFactory;
            this._congressPresentationTypeModelFactory = congressPresentationTypeModelFactory;

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
        public virtual CongressModel PrepareCongressModel(Congress entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<CongressModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">Congress post model</param>
        /// <param name="congress">Congress post entity</param>
        /// <param name="prepareComments">Whether to prepare Congress comments</param>
        public virtual void PrepareCongressModel(CongressModel model, Congress entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            model.CongressPresentationTypes = null;
            model.CongressPaperTypes = null;

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            model.CongressPaperTypes = _congressPaperTypeModelFactory.PrepareCongressPaperTypeListModel(new CongressPaperTypePagingFilteringModel() { CongressId = model.Id });
            model.CongressPresentationTypes = _congressPresentationTypeModelFactory.PrepareCongressPresentationTypeListModel(new CongressPresentationTypePagingFilteringModel() { CongressId = model.Id });
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public CongressListModel PrepareCongressListModel(CongressPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new CongressListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 100;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var congresses = _congressService.GetAllByFilters(command.Title, command.IsArchived, command.IsActive, command.Deleted, command.ShowOn, null, null, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(congresses);

            model.Congresses = congresses
                .Select(x =>
                {
                    var entityModel = x.ToModel<CongressModel>();
                    PrepareCongressModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

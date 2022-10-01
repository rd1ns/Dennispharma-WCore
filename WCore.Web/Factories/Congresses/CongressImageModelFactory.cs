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
    public interface ICongressImageModelFactory
    {
        void PrepareCongressImageModel(CongressImageModel model, CongressImage entity);
        CongressImageModel PrepareCongressImageModel(CongressImage entity);
        CongressImageListModel PrepareCongressImageListModel(CongressImagePagingFilteringModel command);
    }

    public class CongressImageModelFactory : ICongressImageModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ICongressImageService _congressImageService;
        private readonly ICongressService _congressService;

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
        public CongressImageModelFactory(UserSettings userSettings,
        ICongressImageService congressImageService,
        ICongressService congressService,
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
            this._congressImageService = congressImageService;

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
        public virtual CongressImageModel PrepareCongressImageModel(CongressImage entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<CongressImageModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Description = _localizationService.GetLocalized(entity, x => x.Description);
            model.Slogan = _localizationService.GetLocalized(entity, x => x.Slogan);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">CongressImage post model</param>
        /// <param name="activity">CongressImage post entity</param>
        /// <param name="prepareComments">Whether to prepare CongressImage comments</param>
        public virtual void PrepareCongressImageModel(CongressImageModel model, CongressImage entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Description = _localizationService.GetLocalized(entity, x => x.Description);
            model.Slogan = _localizationService.GetLocalized(entity, x => x.Slogan);

        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public CongressImageListModel PrepareCongressImageListModel(CongressImagePagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new CongressImageListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<CongressImage> congressImages = _congressImageService.GetAllByFilters(command.CongressId, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(congressImages);

            model.CongressImages = congressImages
                .Select(x =>
                {
                    var entityModel = x.ToModel<CongressImageModel>();
                    PrepareCongressImageModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}
using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Academies;
using WCore.Services.Common;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Academies;

namespace WCore.Web.Factories
{
    public interface IAcademyVideoModelFactory
    {
        void PrepareAcademyVideoModel(AcademyVideoModel model, AcademyVideo entity);
        AcademyVideoModel PrepareAcademyVideoModel(AcademyVideo entity);
        AcademyVideoListModel PrepareAcademyVideoListModel(AcademyVideoPagingFilteringModel command);
    }

    public class AcademyVideoModelFactory : IAcademyVideoModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IAcademyVideoService _academyVideoService;
        private readonly IAcademyService _academyService;

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
        public AcademyVideoModelFactory(UserSettings userSettings,
        IAcademyVideoService academyVideoService,
        IAcademyService academyService,
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
            this._academyService = academyService;
            this._academyVideoService = academyVideoService;

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
        public virtual AcademyVideoModel PrepareAcademyVideoModel(AcademyVideo entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<AcademyVideoModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Path = _localizationService.GetLocalized(entity, x => x.Path);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">AcademyVideo post model</param>
        /// <param name="activity">AcademyVideo post entity</param>
        /// <param name="prepareComments">Whether to prepare AcademyVideo comments</param>
        public virtual void PrepareAcademyVideoModel(AcademyVideoModel model, AcademyVideo entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Path = _localizationService.GetLocalized(entity, x => x.Path);

        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public AcademyVideoListModel PrepareAcademyVideoListModel(AcademyVideoPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new AcademyVideoListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<AcademyVideo> academyVideos = _academyVideoService.GetAllByFilters(command.AcademyId, command.AcademyVideoResource, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(academyVideos);

            model.AcademyVideos = academyVideos
                .Select(x =>
                {
                    var entityModel = x.ToModel<AcademyVideoModel>();
                    PrepareAcademyVideoModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}
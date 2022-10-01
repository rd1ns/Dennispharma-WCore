using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Academies;
using WCore.Services.Common;
using WCore.Services.Galleries;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Academies;
using WCore.Web.Models.Galleries;

namespace WCore.Web.Factories
{
    public interface IAcademyImageModelFactory
    {
        void PrepareAcademyImageModel(AcademyImageModel model, AcademyImage entity);
        AcademyImageModel PrepareAcademyImageModel(AcademyImage entity);
        AcademyImageListModel PrepareAcademyImageListModel(AcademyImagePagingFilteringModel command);
    }

    public class AcademyImageModelFactory : IAcademyImageModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IAcademyImageService _academyImageService;
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
        public AcademyImageModelFactory(UserSettings userSettings,
        IAcademyImageService academyImageService,
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
            this._academyImageService = academyImageService;

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
        public virtual AcademyImageModel PrepareAcademyImageModel(AcademyImage entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<AcademyImageModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Description = _localizationService.GetLocalized(entity, x => x.Description);
            model.Slogan = _localizationService.GetLocalized(entity, x => x.Slogan);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">AcademyImage post model</param>
        /// <param name="activity">AcademyImage post entity</param>
        /// <param name="prepareComments">Whether to prepare AcademyImage comments</param>
        public virtual void PrepareAcademyImageModel(AcademyImageModel model, AcademyImage entity)
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
        public AcademyImageListModel PrepareAcademyImageListModel(AcademyImagePagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new AcademyImageListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<AcademyImage> academyImages = _academyImageService.GetAllByFilters(command.AcademyId, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(academyImages);

            model.AcademyImages = academyImages
                .Select(x =>
                {
                    var entityModel = x.ToModel<AcademyImageModel>();
                    PrepareAcademyImageModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}
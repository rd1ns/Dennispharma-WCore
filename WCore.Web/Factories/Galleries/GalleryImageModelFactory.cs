using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Common;
using WCore.Services.Galleries;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Galleries;

namespace WCore.Web.Factories
{
    public interface IGalleryImageModelFactory
    {
        void PrepareGalleryImageModel(GalleryImageModel model, GalleryImage entity);
        GalleryImageModel PrepareGalleryImageModel(GalleryImage entity);
        GalleryImageListModel PrepareGalleryImageListModel(GalleryImagePagingFilteringModel command);
    }

    public class GalleryImageModelFactory : IGalleryImageModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IGalleryImageService _galleryImageService;
        private readonly IGalleryService _galleryService;

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
        public GalleryImageModelFactory(UserSettings userSettings,
        IGalleryImageService galleryImageService,
        IGalleryService galleryService,
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
            this._galleryService = galleryService;
            this._galleryImageService = galleryImageService;

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
        public virtual GalleryImageModel PrepareGalleryImageModel(GalleryImage entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<GalleryImageModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Description = _localizationService.GetLocalized(entity, x => x.Description);
            model.Slogan = _localizationService.GetLocalized(entity, x => x.Slogan);
            model.Link = _localizationService.GetLocalized(entity, x => x.Link);
            model.LinkText = _localizationService.GetLocalized(entity, x => x.LinkText);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">GalleryImage post model</param>
        /// <param name="activity">GalleryImage post entity</param>
        /// <param name="prepareComments">Whether to prepare GalleryImage comments</param>
        public virtual void PrepareGalleryImageModel(GalleryImageModel model, GalleryImage entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Description = _localizationService.GetLocalized(entity, x => x.Description);
            model.Slogan = _localizationService.GetLocalized(entity, x => x.Slogan);
            model.Link = _localizationService.GetLocalized(entity, x => x.Link);
            model.LinkText = _localizationService.GetLocalized(entity, x => x.LinkText);

        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public GalleryImageListModel PrepareGalleryImageListModel(GalleryImagePagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new GalleryImageListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<GalleryImage> galleyImages = _galleryImageService.GetAllByFilters(command.GalleryId, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(galleyImages);

            model.GalleryImages = galleyImages
                .Select(x =>
                {
                    var entityModel = x.ToModel<GalleryImageModel>();
                    PrepareGalleryImageModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}
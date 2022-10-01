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
    public interface IGalleryModelFactory
    {
        void PrepareGalleryModel(GalleryModel model, Gallery entity);
        GalleryModel PrepareGalleryModel(GalleryType galleryType);
        GalleryModel PrepareGalleryModel(int galleryId);
        GalleryModel PrepareGalleryModel(Gallery entity);
        GalleryListModel PrepareGalleryListModel(GalleryPagingFilteringModel command);
    }

    public class GalleryModelFactory : IGalleryModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IGalleryService _galleryService;
        private readonly IGalleryImageService _galleryImageService;
        private readonly IGalleryImageModelFactory _galleryImageModelFactory;

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
        public GalleryModelFactory(UserSettings userSettings,
        IGalleryService galleryService,
        IGalleryImageService galleryImageService,
        IGalleryImageModelFactory galleryImageModelFactory,
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
            this._galleryImageModelFactory = galleryImageModelFactory;


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
        public virtual GalleryModel PrepareGalleryModel(GalleryType galleryType)
        {
            var entity = _galleryService.GetByGalleryType(galleryType);

            if (entity == null)
                return null;

            var model = entity.ToModel<GalleryModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.ShortBody = _localizationService.GetLocalized(entity, x => x.ShortBody);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);

            model.GalleryImages = _galleryImageService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<GalleryImageModel>();
                _galleryImageModelFactory.PrepareGalleryImageModel(gi, o);
                return gi;
            }).ToList();

            return model;
        }
        public virtual GalleryModel PrepareGalleryModel(int galleryId)
        {
            var entity = _galleryService.GetById(galleryId);

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<GalleryModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.ShortBody = _localizationService.GetLocalized(entity, x => x.ShortBody);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);

            model.GalleryImages = _galleryImageService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<GalleryImageModel>();
                _galleryImageModelFactory.PrepareGalleryImageModel(gi, o);
                return gi;
            }).ToList();

            return model;
        }
        public virtual GalleryModel PrepareGalleryModel(Gallery entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<GalleryModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.ShortBody = _localizationService.GetLocalized(entity, x => x.ShortBody);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);

            model.GalleryImages = _galleryImageService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<GalleryImageModel>();
                _galleryImageModelFactory.PrepareGalleryImageModel(gi, o);
                return gi;
            }).ToList();

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">Gallery post model</param>
        /// <param name="activity">Gallery post entity</param>
        /// <param name="prepareComments">Whether to prepare Gallery comments</param>
        public virtual void PrepareGalleryModel(GalleryModel model, Gallery entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.ShortBody = _localizationService.GetLocalized(entity, x => x.ShortBody);
            model.Image = _localizationService.GetLocalized(entity, x => x.Image);

            model.GalleryImages = _galleryImageService.GetAllByFilters(model.Id).Select(o =>
            {
                var gi = o.ToModel<GalleryImageModel>();
                _galleryImageModelFactory.PrepareGalleryImageModel(gi, o);
                return gi;
            }).ToList();
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public GalleryListModel PrepareGalleryListModel(GalleryPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new GalleryListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<Gallery> galleries = _galleryService.GetAllByFilters(command.GalleryType, command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(galleries);

            model.Galleries = galleries
                .Select(x =>
                {
                    var entityModel = x.ToModel<GalleryModel>();
                    PrepareGalleryModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}
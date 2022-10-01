using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Newses;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Academies;
using WCore.Services.Common;
using WCore.Services.Galleries;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Newses;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Academies;
using WCore.Web.Models.Galleries;
using WCore.Web.Models.Newses;

namespace WCore.Web.Factories
{
    public interface INewsImageModelFactory
    {
        void PrepareNewsImageModel(NewsImageModel model, NewsImage entity);
        NewsImageModel PrepareNewsImageModel(NewsImage entity);
        NewsImageListModel PrepareNewsImageListModel(NewsImagePagingFilteringModel command);
    }

    public class NewsImageModelFactory : INewsImageModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly INewsImageService _newsImageService;
        private readonly INewsService _newsService;

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
        public NewsImageModelFactory(UserSettings userSettings,
        INewsImageService newsImageService,
        INewsService newsService,
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
            this._newsService = newsService;
            this._newsImageService = newsImageService;

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
        public virtual NewsImageModel PrepareNewsImageModel(NewsImage entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<NewsImageModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Description = _localizationService.GetLocalized(entity, x => x.Description);
            model.Slogan = _localizationService.GetLocalized(entity, x => x.Slogan);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">NewsImage post model</param>
        /// <param name="activity">NewsImage post entity</param>
        /// <param name="prepareComments">Whether to prepare NewsImage comments</param>
        public virtual void PrepareNewsImageModel(NewsImageModel model, NewsImage entity)
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
        public NewsImageListModel PrepareNewsImageListModel(NewsImagePagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new NewsImageListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<NewsImage> newsImages = _newsImageService.GetAllByFilters(command.NewsId, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(newsImages);

            model.NewsImages = newsImages
                .Select(x =>
                {
                    var entityModel = x.ToModel<NewsImageModel>();
                    PrepareNewsImageModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}
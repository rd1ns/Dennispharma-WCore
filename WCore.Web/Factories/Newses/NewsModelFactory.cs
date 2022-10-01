using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Newses;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Common;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Newses;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Newses;

namespace WCore.Web.Factories
{
    public interface INewsModelFactory
    {
        void PrepareNewsModel(NewsModel model, News entity);
        NewsModel PrepareNewsModel(News entity);
        NewsListModel PrepareNewsListModel(NewsPagingFilteringModel command);
    }

    public class NewsModelFactory : INewsModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly INewsService _newsService;
        private readonly INewsCategoryService _newsCategoryService;
        private readonly INewsCategoryModelFactory _newsCategoryModelFactory;

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
        public NewsModelFactory(UserSettings userSettings,
        INewsService newsService,
        INewsCategoryService newsCategoryService,
        INewsCategoryModelFactory newsCategoryModelFactory,
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
            this._newsCategoryService = newsCategoryService;
            this._newsCategoryModelFactory = newsCategoryModelFactory;

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
        public virtual NewsModel PrepareNewsModel(News entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<NewsModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            var newsCategory = _newsCategoryService.GetById(entity.NewsCategoryId);
            model.NewsCategory = _newsCategoryModelFactory.PrepareNewsCategoryModel(newsCategory);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">News post model</param>
        /// <param name="news">News post entity</param>
        /// <param name="prepareComments">Whether to prepare News comments</param>
        public virtual void PrepareNewsModel(NewsModel model, News entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            var newsCategory = _newsCategoryService.GetById(entity.NewsCategoryId);
            if (newsCategory != null)
            {
                model.NewsCategory = _newsCategoryModelFactory.PrepareNewsCategoryModel(newsCategory);
            }
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public NewsListModel PrepareNewsListModel(NewsPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new NewsListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var newses = _newsService.GetAllByFilters(command.NewsCategoryId, command.Title, command.IsArchived, command.IsActive, command.Deleted, command.ShowOn, command.ShowOnHome, null, null, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(newses);

            model.Newses = newses
                .Select(x =>
                {
                    var entityModel = x.ToModel<NewsModel>();
                    PrepareNewsModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

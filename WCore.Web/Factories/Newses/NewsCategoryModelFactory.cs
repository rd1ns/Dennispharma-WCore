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
    public interface INewsCategoryModelFactory
    {
        void PrepareNewsCategoryModel(NewsCategoryModel model, NewsCategory entity);
        NewsCategoryModel PrepareNewsCategoryModel(NewsCategory entity);
        NewsCategoryListModel PrepareNewsCategoryListModel(NewsCategoryPagingFilteringModel command);
    }

    public class NewsCategoryModelFactory : INewsCategoryModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly INewsService _newsService;
        private readonly INewsCategoryService _newsCategoryService;

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
        public NewsCategoryModelFactory(UserSettings userSettings,
        INewsCategoryService newsCategoryService,
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
            this._newsCategoryService = newsCategoryService;

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
        public virtual NewsCategoryModel PrepareNewsCategoryModel(NewsCategory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<NewsCategoryModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">NewsCategory post model</param>
        /// <param name="newsCategory">NewsCategory post entity</param>
        /// <param name="prepareComments">Whether to prepare NewsCategory comments</param>
        public virtual void PrepareNewsCategoryModel(NewsCategoryModel model, NewsCategory entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public NewsCategoryListModel PrepareNewsCategoryListModel(NewsCategoryPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new NewsCategoryListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var newsCategories = _newsCategoryService.GetAllByFilters(command.Title, command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(newsCategories);

            model.NewsCategories = newsCategories
                .Select(x =>
                {
                    var entityModel = x.ToModel<NewsCategoryModel>();
                    PrepareNewsCategoryModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

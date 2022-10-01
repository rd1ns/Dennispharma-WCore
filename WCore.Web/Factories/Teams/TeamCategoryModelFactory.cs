using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Teams;
using WCore.Core.Domain.Users;
using WCore.Services.Common;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Teams;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Teams;

namespace WCore.Web.Factories
{
    public interface ITeamCategoryModelFactory
    {
        void PrepareTeamCategoryModel(TeamCategoryModel model, TeamCategory entity);
        TeamCategoryModel PrepareTeamCategoryModel(TeamCategory entity);
        TeamCategoryListModel PrepareTeamCategoryListModel(TeamCategoryPagingFilteringModel command);
    }

    public class TeamCategoryModelFactory : ITeamCategoryModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ITeamService _teamService;
        private readonly ITeamCategoryService _teamCategoryService;

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
        public TeamCategoryModelFactory(UserSettings userSettings,
        ITeamCategoryService teamCategoryService,
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
            this._teamCategoryService = teamCategoryService;

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
        public virtual TeamCategoryModel PrepareTeamCategoryModel(TeamCategory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<TeamCategoryModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">TeamCategory post model</param>
        /// <param name="teamCategory">TeamCategory post entity</param>
        /// <param name="prepareComments">Whether to prepare TeamCategory comments</param>
        public virtual void PrepareTeamCategoryModel(TeamCategoryModel model, TeamCategory entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);


            model.SubTeamCategories = _teamCategoryService.GetAllByFilters(ParentId: model.Id)
                .Select(x =>
                {
                    var entityModel = x.ToModel<TeamCategoryModel>();
                    PrepareTeamCategoryModel(entityModel, x);
                    return entityModel;
                }).ToList();

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public TeamCategoryListModel PrepareTeamCategoryListModel(TeamCategoryPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new TeamCategoryListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var teamCategories = _teamCategoryService.GetAllByFilters(command.ParentId,command.Title, command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(teamCategories);

            model.TeamCategories = teamCategories
                .Select(x =>
                {
                    var entityModel = x.ToModel<TeamCategoryModel>();
                    PrepareTeamCategoryModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

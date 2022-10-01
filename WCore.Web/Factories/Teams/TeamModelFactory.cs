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
    public interface ITeamModelFactory
    {
        void PrepareTeamModel(TeamModel model, Team entity);
        TeamModel PrepareTeamModel(Team entity);
        TeamListModel PrepareTeamListModel(TeamPagingFilteringModel command);
    }

    public class TeamModelFactory : ITeamModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ITeamService _teamService;
        private readonly ITeamCategoryService _teamCategoryService;
        private readonly ITeamCategoryModelFactory _teamCategoryModelFactory;

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
        public TeamModelFactory(UserSettings userSettings,
        ITeamService teamService,
        ITeamCategoryService teamCategoryService,
        ITeamCategoryModelFactory teamCategoryModelFactory,
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
            this._teamService = teamService;
            this._teamCategoryService = teamCategoryService;
            this._teamCategoryModelFactory = teamCategoryModelFactory;

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
        public virtual TeamModel PrepareTeamModel(Team entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<TeamModel>();

            var teamCategory = _teamCategoryService.GetById(entity.TeamCategoryId);
            model.TeamCategory = _teamCategoryModelFactory.PrepareTeamCategoryModel(teamCategory);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">Team post model</param>
        /// <param name="team">Team post entity</param>
        /// <param name="prepareComments">Whether to prepare Team comments</param>
        public virtual void PrepareTeamModel(TeamModel model, Team entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var teamCategory = _teamCategoryService.GetById(entity.TeamCategoryId);
            if (teamCategory != null)
            {
                model.TeamCategory = _teamCategoryModelFactory.PrepareTeamCategoryModel(teamCategory);
            }
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public TeamListModel PrepareTeamListModel(TeamPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new TeamListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = int.MaxValue;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var teames = _teamService.GetAllByFilters(command.TeamCategoryId, command.Name, command.Surname, command.IsActive, command.Deleted, command.ShowOn, command.ShowOnHome,  command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(teames);

            model.Teams = teames
                .Select(x =>
                {
                    var entityModel = x.ToModel<TeamModel>();
                    PrepareTeamModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

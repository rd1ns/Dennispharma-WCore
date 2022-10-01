
using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Settings;
using WCore.Services.Teams;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Factories;
using WCore.Web.Models.Teams;

namespace WCore.Web.Controllers
{
    public class TeamCategoryController : BasePublicController
    {
        #region Fields
        private readonly ITeamCategoryService _teamCategoryService;
        private readonly ITeamCategoryModelFactory _teamCategoryModelFactory;
        private readonly ITeamModelFactory _teamModelFactory;


        private readonly IUserModelFactory _userModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public TeamCategoryController(ITeamCategoryService teamCategoryService,
            ITeamCategoryModelFactory teamCategoryModelFactory,
            ITeamModelFactory teamModelFactory,
            IUserModelFactory userModelFactory,
            IWorkContext workContext,
            ISettingService settingService)
        {
            this._teamCategoryService = teamCategoryService;
            this._teamCategoryModelFactory = teamCategoryModelFactory;
            this._teamModelFactory = teamModelFactory;

            this._userModelFactory = userModelFactory;
            this._workContext = workContext;
            this._settingService = settingService;
        }
        #endregion

        #region Methods
        public IActionResult Details(int teamCategoryid)
        {
            var teamCategory = _teamCategoryService.GetById(teamCategoryid, cache => default);
            if (teamCategory == null)
                return InvokeHttp404();

            if ((teamCategory.Deleted || !teamCategory.IsActive) && _workContext.CurrentUser.RoleGroup.RoleGroupType != Core.Domain.Roles.RoleGroupType.WCore)
                return NotFound();

            var model = teamCategory.ToModel<TeamCategoryModel>();
            _teamCategoryModelFactory.PrepareTeamCategoryModel(model, teamCategory);

            model.PageTitle = new Models.PageTitleModel()
            {
                Title = model.Title
            };


            var teamPaging = new TeamPagingFilteringModel()
            {
                TeamCategoryId = teamCategoryid
            };
            model.Teams = _teamModelFactory.PrepareTeamListModel(teamPaging);

            return View(model);
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Localization;
using WCore.Services.Teams;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Teams;

namespace WCore.Web.ViewComponents
{
    public class TeamCategoryViewComponent : ViewComponent
    {
        private readonly ITeamCategoryService _teamCategoryService;
        private readonly ITeamCategoryModelFactory _teamCategoryModelFactory;
        private readonly ITeamService _teamService;
        private readonly ITeamModelFactory _teamModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public TeamCategoryViewComponent(ITeamCategoryService teamCategoryService,
            ITeamCategoryModelFactory teamCategoryModelFactory,
            ITeamService teamService,
            ITeamModelFactory teamModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._teamCategoryService = teamCategoryService;
            this._teamCategoryModelFactory = teamCategoryModelFactory;
            this._teamService = teamService;
            this._teamModelFactory = teamModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke(int teamCategoryId)
        {
            var teamCategory = _teamCategoryService.GetById(teamCategoryId);

            var model = teamCategory.ToModel<TeamCategoryModel>();

            _teamCategoryModelFactory.PrepareTeamCategoryModel(model, teamCategory);


            var teamPaging = new TeamPagingFilteringModel()
            {
                TeamCategoryId = teamCategoryId
            };
            model.Teams = _teamModelFactory.PrepareTeamListModel(teamPaging);

            return View(model);
        }
    }
}

using System;
using WCore.Core;
using WCore.Core.Domain.Newses;
using WCore.Core.Domain.Teams;

namespace WCore.Services.Teams
{
    public interface ITeamService : IRepository<Team>
    {
        IPagedList<Team> GetAllByFilters(int? TeamCategoryId = null,
            string Name = "",
            string Surname = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            bool? ShowOnHome = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface ITeamCategoryService : IRepository<TeamCategory>
    {
        IPagedList<TeamCategory> GetAllByFilters(int? ParentId = 0, 
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
}

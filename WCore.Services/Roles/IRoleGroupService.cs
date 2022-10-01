using WCore.Core;
using WCore.Core.Domain.Roles;

namespace WCore.Services.Roles
{
    public interface IRoleGroupService : IRepository<RoleGroup>
    {
        IPagedList<RoleGroup> GetAllByFilters(string searchValue = "", RoleGroupType? roleGroupType = null, int skip = 0, int take = 10);
    }
}

using WCore.Core;
using WCore.Core.Domain.Roles;

namespace WCore.Services.Roles
{
    public interface IRoleService : IRepository<Role>
    {
        IPagedList<Role> GetAllByFilters(int? roleGroupId = null, int? menuId = null, int skip = 0, int take = 10);
        Role GetRoleByMenuId(int roleGroupId, int menuId);
        Role GetMenuIdRoleGroupId(int roleGroupId, int menuId);
        Role GetRoleByControllerAndActionName(int roleGroupId, string controller, string Action);
    }
}

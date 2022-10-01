using WCore.Core;
using WCore.Core.Domain.Roles;
using System.Collections.Generic;
using System.Linq;

namespace WCore.Services.Roles
{
    public class RoleService : Repository<Role>, IRoleService
    {
        public RoleService(WCoreContext context) : base(context)
        {
        }
        public IPagedList<Role> GetAllByFilters(int? roleGroupId = null, int? menuId = null, int skip = 0, int take = 10)
        {
            IQueryable<Role> recordsFiltered = context.Set<Role>();

            if (roleGroupId.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.RoleGroupId == roleGroupId.Value);

            if (menuId.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.MenuId == menuId.Value);

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<Role>().Count();

            var data = recordsFiltered.Skip(skip).Take(take).ToList();

            return new PagedList<Role>(data, skip, take, recordsFilteredCount);
        }
        public Role GetRoleByMenuId(int roleGroupId, int menuId)
        {
            var query = context.Set<Role>()
                .Join(context.Set<Menu>(), role => role.MenuId, menu => menu.Id, (role, menu) => new { Menu = menu, Role = role })
                .Where(role => role.Role.RoleGroupId == roleGroupId).ToList().Select(o => o.Role).ToList();

            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        public Role GetMenuIdRoleGroupId(int roleGroupId, int menuId)
        {
            return context.Roles.FirstOrDefault(o => o.RoleGroupId == roleGroupId && o.MenuId == menuId);
        }
        public Role GetRoleByControllerAndActionName(int roleGroupId, string controller, string Action)
        {
            var query = context.Set<Role>()
                .Join(context.Set<Menu>(), role => role.MenuId, menu => menu.Id, (role, menu) => new { Menu = menu, Role = role })
                .Where(role => role.Role.RoleGroupId == roleGroupId && role.Menu.Controller == controller && role.Menu.Action == Action).ToList().Select(o => o.Role).ToList();

            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;

        }
    }
}

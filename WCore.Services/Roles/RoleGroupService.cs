using WCore.Core;
using WCore.Core.Domain.Roles;
using System.Collections.Generic;
using System.Linq;

namespace WCore.Services.Roles
{
    public class RoleGroupService : Repository<RoleGroup>, IRoleGroupService
    {
        public RoleGroupService(WCoreContext context) : base(context)
        {
        }
        public IPagedList<RoleGroup> GetAllByFilters(string searchValue = "", RoleGroupType? roleGroupType = null, int skip = 0, int take = 10)
        {
            IQueryable<RoleGroup> recordsFiltered = context.Set<RoleGroup>();

            if (!string.IsNullOrEmpty(searchValue))
                recordsFiltered = recordsFiltered.Where(o => o.Name.Contains(searchValue));

            if (roleGroupType.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.RoleGroupType == roleGroupType);

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<RoleGroup>().Count();

            var data = recordsFiltered.Skip(skip).Take(take).ToList();

            return new PagedList<RoleGroup>(data, skip, take, recordsFilteredCount);
        }
    }
}

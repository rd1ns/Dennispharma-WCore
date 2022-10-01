using WCore.Core;
using WCore.Core.Domain.Roles;
using System.Linq;

namespace WCore.Services.Roles
{
    public class UserRoleService : Repository<UserRole>, IUserRoleService
    {
        public UserRoleService(WCoreContext context) : base(context)
        {
        }

        public IPagedList<UserRole> GetAllByFilters(int? userId = null, int skip = 0, int take = 10)
        {
            IQueryable<UserRole> recordsFiltered = context.Set<UserRole>();

            if (userId.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.UserId == userId.Value);

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<UserRole>().Count();

            var data = recordsFiltered.Skip(skip).Take(take).ToList();

            return new PagedList<UserRole>(data, skip, take, recordsFilteredCount);
        }

        public UserRole GetByDataTokenAndAction(int userId, string dataToken, string action)
        {
            return context.Set<UserRole>().FirstOrDefault(o => o.UserId == userId && o.DataToken == dataToken && o.Action == action);
        }
    }
}

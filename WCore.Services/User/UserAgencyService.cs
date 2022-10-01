using WCore.Core;
using WCore.Core.Domain.Users;
using System.Linq;

namespace WCore.Services.Users
{
    public class UserAgencyService : Repository<UserAgency>, IUserAgencyService
    {
        public UserAgencyService(WCoreContext context) : base(context)
        {
        }

        public IPagedList<UserAgency> GetAllByFilters(string Name = "", bool? IsActive = null, bool? Deleted = null,int skip = 0,int take = int.MaxValue)
        {
            IQueryable<UserAgency> recordsFiltered = context.Set<UserAgency>();

            if (!string.IsNullOrEmpty(Name))
                recordsFiltered = recordsFiltered.Where(a => a.Name.Contains(Name));

            if (IsActive.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsActive == IsActive);

            if (Deleted.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.Deleted == Deleted);

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<UserAgency>().Count();

            var data = recordsFiltered.OrderByDescending(o => o.IsActive).Skip(skip).Take(take).ToList();

            return new PagedList<UserAgency>(data, skip, take, recordsFilteredCount);
        }
    }
}

using WCore.Core;
using WCore.Core.Domain.Users;
using System;
using System.Data.Entity;
using System.Linq;

namespace WCore.Services.Users
{
    public class UserAgencyAuthorizationService : Repository<UserAgencyAuthorization>, IUserAgencyAuthorizationService
    {
        public UserAgencyAuthorizationService(WCoreContext context) : base(context)
        {
        }

        public IPagedList<UserAgencyAuthorization> GetAllByFilters(
            int? userAgencyId = null, 
            bool? IsRead = null, 
            bool? IsCreate = null, 
            bool? IsUpdate = null,
            bool? IsDelete = null)
        {
            IQueryable<UserAgencyAuthorization> recordsFiltered = context.Set<UserAgencyAuthorization>();

            if (userAgencyId.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.UserAgencyId == userAgencyId);

            if (IsRead.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsRead == IsRead);

            if (IsCreate.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsCreate == IsCreate);

            if (IsUpdate.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsUpdate == IsUpdate);

            if (IsDelete.HasValue)
                recordsFiltered = recordsFiltered.Where(a => a.IsDelete == IsDelete);

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<UserAgencyAuthorization>().Count();

            var data = recordsFiltered.ToList();

            return new PagedList<UserAgencyAuthorization>(data, 0, int.MaxValue, recordsFilteredCount);
        }
    }
}

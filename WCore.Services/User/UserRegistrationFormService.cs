using WCore.Core;
using WCore.Core.Domain.Users;
using System.Linq;

namespace WCore.Services.Users
{
    public class UserRegistrationFormService : Repository<UserRegistrationForm>, IUserRegistrationFormService
    {
        public UserRegistrationFormService(WCoreContext context) : base(context)
        {
        }

        public IPagedList<UserRegistrationForm> GetAllByFilters(string FirstName = "", int skip = 0, int take = int.MaxValue)
        {
            IQueryable<UserRegistrationForm> recordsFiltered = context.Set<UserRegistrationForm>();

            if (!string.IsNullOrEmpty(FirstName))
                recordsFiltered = recordsFiltered.Where(a => a.FirstName.Contains(FirstName));

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<UserRegistrationForm>().Count();

            var data = recordsFiltered.OrderByDescending(o => o.CreatedOn).Skip(skip).Take(take).ToList();

            return new PagedList<UserRegistrationForm>(data, skip, take, recordsFilteredCount);
        }
    }
}

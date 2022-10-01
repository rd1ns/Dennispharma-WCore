using WCore.Core;
using WCore.Core.Domain.Roles;
using System.Linq;

namespace WCore.Services.Roles
{
    public class RoutingService : Repository<Routing>, IRoutingService
    {
        public RoutingService(WCoreContext context) : base(context)
        {
        }
        public IPagedList<Routing> GetAllByFilters(string searchValue = "", int skip = 0, int take = 10)
        {
            IQueryable<Routing> recordsFiltered = context.Set<Routing>();

            if (!string.IsNullOrEmpty(searchValue))
                recordsFiltered = recordsFiltered.Where(o => o.Name.Contains(searchValue));

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<Routing>().Count();

            var data = recordsFiltered.Skip(skip).Take(take).ToList();

            return new PagedList<Routing>(data, skip, take, recordsFilteredCount);
        }
        public virtual Routing GetRouting(string ControllerName = "", string ActionName = "")
        {
            IQueryable<Routing> recordsFiltered = context.Set<Routing>();

            if (!string.IsNullOrEmpty(ControllerName))
                recordsFiltered = recordsFiltered.Where(o => o.Controller == ControllerName);

            if (!string.IsNullOrEmpty(ActionName))
                recordsFiltered = recordsFiltered.Where(o => o.Action == ActionName);

            return recordsFiltered.FirstOrDefault();
        }
    }
}

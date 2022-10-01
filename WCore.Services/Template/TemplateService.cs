using WCore.Core;
using WCore.Core.Domain.Templates;
using System.Collections.Generic;
using System.Linq;

namespace WCore.Services.Templates
{
    public class TemplateService : Repository<Template>, ITemplateService
    {
        public TemplateService(WCoreContext context) : base(context)
        {
        }

        public IPagedList<Template> GetAllByFilters(int? userId = null, string searchValue = "", string sortColumnName = "", string sortColumnDirection = "", int skip = 0, int take = 10)
        {
            IQueryable<Template> recordsFiltered = context.Set<Template>();


            if (userId.HasValue)
                recordsFiltered = recordsFiltered.Where(o => o.CreatedUserId == userId.Value);

            int recordsFilteredCount = recordsFiltered.Count();

            int recordsTotalCount = context.Set<Template>().Count();


            var data = recordsFiltered.OrderByDescending(o => o.IsActive).ThenByDescending(o => o.CreatedOn).ToList();

            return new PagedList<Template>(data, 0, 10, recordsFilteredCount);
        }
    }
}

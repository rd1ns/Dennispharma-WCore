using WCore.Core;
using WCore.Core.Domain.Templates;

namespace WCore.Services.Templates
{
    public interface ITemplateService : IRepository<Template>
    {
        IPagedList<Template> GetAllByFilters(int? userId = null, string searchValue = "", string sortColumnName = "", string sortColumnDirection = "", int skip = 0, int take = 10);
    }
}

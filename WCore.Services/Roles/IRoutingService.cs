using WCore.Core;
using WCore.Core.Domain.Roles;

namespace WCore.Services.Roles
{
    public interface IRoutingService : IRepository<Routing>
    {
        IPagedList<Routing> GetAllByFilters(string searchValue = "", int skip = 0, int take = 10);
        Routing GetRouting(string ControllerName = "", string ActionName = "");
    }
}

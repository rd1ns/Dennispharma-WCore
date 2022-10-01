using WCore.Core;
using WCore.Core.Domain.Common;

namespace WCore.Services.Common
{
    public interface ICityService : IRepository<City>
    {

        IPagedList<City> GetAllByFilters(int CountryId,
            string Name = "",
            bool? IsActive = null,
            bool? Deleted = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
}

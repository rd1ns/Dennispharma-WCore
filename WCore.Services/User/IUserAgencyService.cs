using WCore.Core;
using WCore.Core.Domain.Users;

namespace WCore.Services.Users
{
    public interface IUserAgencyService : IRepository<UserAgency>
    {
        IPagedList<UserAgency> GetAllByFilters(string Name = "", bool? IsActive = null, bool? Deleted = null, int skip = 0, int take = int.MaxValue);


    }
}

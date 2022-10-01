using WCore.Core;
using WCore.Core.Domain.Users;

namespace WCore.Services.Users
{
    public interface IUserAgencyAuthorizationService : IRepository<UserAgencyAuthorization>
    {
        IPagedList<UserAgencyAuthorization> GetAllByFilters(
            int? userAgencyId = null,
            bool? IsRead = null,
            bool? IsCreate = null,
            bool? IsUpdate = null,
            bool? IsDelete = null);


    }
}

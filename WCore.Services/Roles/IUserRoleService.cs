using WCore.Core;
using WCore.Core.Domain.Roles;

namespace WCore.Services.Roles
{
    public interface IUserRoleService : IRepository<UserRole>
    {
        IPagedList<UserRole> GetAllByFilters(int? userId = null, int skip = 0, int take = 10);

        UserRole GetByDataTokenAndAction(int userId, string dataToken, string action);
    }
}

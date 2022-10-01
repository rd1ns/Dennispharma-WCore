using WCore.Core;
using WCore.Core.Domain.Users;

namespace WCore.Services.Users
{
    public interface IUserRegistrationFormService : IRepository<UserRegistrationForm>
    {
        IPagedList<UserRegistrationForm> GetAllByFilters(string FirstName = "", int skip = 0, int take = int.MaxValue);

    }
}

using WCore.Framework.Models;
using System.Collections.Generic;

namespace WCore.Web.Models.Users
{
    /// <summary>
    /// Represents a currency list model
    /// </summary>
    public partial class UserListModel : BasePagedListModel<UserModel>
    {
        public UserListModel()
        {
            PagingFilteringContext = new UserPagingFilteringModel();
            Users = new List<UserModel>();
        }

        public int WorkingLanguageId { get; set; }
        public UserPagingFilteringModel PagingFilteringContext { get; set; }
        public List<UserModel> Users { get; set; }
    }
}

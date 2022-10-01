using System.Collections.Generic;
using WCore.Web.Models.Newses;
using WCore.Web.Models.Pages;
using WCore.Web.Models.Users;

namespace WCore.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public virtual UserModel User { get; set; }
        public virtual NewsListModel NewsList { get; set; }
        public virtual PageModel Page { get; set; }
    }
}

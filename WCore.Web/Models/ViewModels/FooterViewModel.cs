using WCore.Web.Models.Pages;
using WCore.Web.Models.Users;

namespace WCore.Web.Models.ViewModels
{
    public class FooterViewModel
    {
        public FooterViewModel()
        {
        }
        public virtual PageListModel PageList { get; set; }
        public virtual UserModel User { get; set; }
    }
}

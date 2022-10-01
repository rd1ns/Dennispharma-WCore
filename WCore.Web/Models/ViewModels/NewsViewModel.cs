using WCore.Web.Models.Newses;

namespace WCore.Web.Models.ViewModels
{
    public class NewsViewModel
    {
        public virtual NewsModel News { get; set; }
        public virtual NewsListModel RecentNewsList { get; set; }
        public virtual NewsCategoryListModel NewsCategoryList { get; set; }
    }
}

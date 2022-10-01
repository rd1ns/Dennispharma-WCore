using System.Collections.Generic;
using WCore.Framework.Models;

namespace WCore.Web.Models.Newses
{
    public partial class NewsListModel : BaseWCoreModel
    {
        public NewsListModel()
        {
            PagingFilteringContext = new NewsPagingFilteringModel();
            Newses = new List<NewsModel>();
        }

        public int WorkingLanguageId { get; set; }
        public NewsPagingFilteringModel PagingFilteringContext { get; set; }
        public List<NewsModel> Newses { get; set; }
    }
    public partial class NewsCategoryListModel : BaseWCoreModel
    {
        public NewsCategoryListModel()
        {
            PagingFilteringContext = new NewsCategoryPagingFilteringModel();
            NewsCategories = new List<NewsCategoryModel>();
        }

        public int WorkingLanguageId { get; set; }
        public NewsCategoryPagingFilteringModel PagingFilteringContext { get; set; }
        public List<NewsCategoryModel> NewsCategories { get; set; }
    }
    public partial class NewsImageListModel : BaseWCoreModel
    {
        public NewsImageListModel()
        {
            PagingFilteringContext = new NewsImagePagingFilteringModel();
            NewsImages = new List<NewsImageModel>();
        }

        public int WorkingLanguageId { get; set; }
        public NewsImagePagingFilteringModel PagingFilteringContext { get; set; }
        public List<NewsImageModel> NewsImages { get; set; }
    }
}

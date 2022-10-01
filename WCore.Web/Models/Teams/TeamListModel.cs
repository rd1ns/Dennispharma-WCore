using System.Collections.Generic;
using WCore.Framework.Models;

namespace WCore.Web.Models.Teams
{
    public partial class TeamListModel : BaseWCoreModel
    {
        public TeamListModel()
        {
            PagingFilteringContext = new TeamPagingFilteringModel();
            Teams = new List<TeamModel>();
        }

        public int WorkingLanguageId { get; set; }
        public TeamPagingFilteringModel PagingFilteringContext { get; set; }
        public List<TeamModel> Teams { get; set; }
    }
    public partial class TeamCategoryListModel : BaseWCoreModel
    {
        public TeamCategoryListModel()
        {
            PagingFilteringContext = new TeamCategoryPagingFilteringModel();
            TeamCategories = new List<TeamCategoryModel>();
        }

        public int WorkingLanguageId { get; set; }
        public TeamCategoryPagingFilteringModel PagingFilteringContext { get; set; }
        public List<TeamCategoryModel> TeamCategories { get; set; }
    }
}

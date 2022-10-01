using System;
using System.Collections.Generic;
using WCore.Framework.Models;

namespace WCore.Web.Models.Teams
{
    public class TeamModel : BaseWCoreEntityModel
    {
        #region Ctor
        public TeamModel()
        {
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }

        public int TeamCategoryId { get; set; }
        public virtual TeamCategoryModel TeamCategory { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }


        public PageTitleModel PageTitle { get; set; }
        #endregion
    }
    public class TeamCategoryModel : BaseWCoreEntityModel
    {
        #region Ctor
        public TeamCategoryModel()
        {
            Teams = new TeamListModel();
            SubTeamCategories = new List<TeamCategoryModel>();

        }
        #endregion

        #region Properties
        public string Title { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public bool ShowOnHome { get; set; }
        public string SeName { get; set; }

        public TeamListModel Teams { get; set; }
        public PageTitleModel PageTitle { get; set; }
        public List<TeamCategoryModel> SubTeamCategories { get; set; }
        #endregion
    }
}

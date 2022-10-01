using WCore.Web.Models.Academies;
using WCore.Web.Models.Newses;

namespace WCore.Web.Models.ViewModels
{
    public class AcademyViewModel
    {
        public virtual AcademyModel Academy { get; set; }
        public virtual AcademyCategoryListModel AcademyCategoryList { get; set; }
    }
}

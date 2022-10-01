using System.Collections.Generic;
using WCore.Framework.Models;

namespace WCore.Web.Models.Academies
{
    public partial class AcademyListModel : BaseWCoreModel
    {
        public AcademyListModel()
        {
            PagingFilteringContext = new AcademyPagingFilteringModel();
            Academies = new List<AcademyModel>();
        }

        public int WorkingLanguageId { get; set; }
        public AcademyPagingFilteringModel PagingFilteringContext { get; set; }
        public List<AcademyModel> Academies { get; set; }
    }
    public partial class AcademyCategoryListModel : BaseWCoreModel
    {
        public AcademyCategoryListModel()
        {
            PagingFilteringContext = new AcademyCategoryPagingFilteringModel();
            AcademyCategories = new List<AcademyCategoryModel>();
        }

        public int WorkingLanguageId { get; set; }
        public AcademyCategoryPagingFilteringModel PagingFilteringContext { get; set; }
        public List<AcademyCategoryModel> AcademyCategories { get; set; }
    }
    public partial class AcademyImageListModel : BaseWCoreModel
    {
        public AcademyImageListModel()
        {
            PagingFilteringContext = new AcademyImagePagingFilteringModel();
            AcademyImages = new List<AcademyImageModel>();
        }

        public int WorkingLanguageId { get; set; }
        public AcademyImagePagingFilteringModel PagingFilteringContext { get; set; }
        public List<AcademyImageModel> AcademyImages { get; set; }
    }
    public partial class AcademyFileListModel : BaseWCoreModel
    {
        public AcademyFileListModel()
        {
            PagingFilteringContext = new AcademyFilePagingFilteringModel();
            AcademyFiles = new List<AcademyFileModel>();
        }

        public int WorkingLanguageId { get; set; }
        public AcademyFilePagingFilteringModel PagingFilteringContext { get; set; }
        public List<AcademyFileModel> AcademyFiles { get; set; }
    }
    public partial class AcademyVideoListModel : BaseWCoreModel
    {
        public AcademyVideoListModel()
        {
            PagingFilteringContext = new AcademyVideoPagingFilteringModel();
            AcademyVideos = new List<AcademyVideoModel>();
        }

        public int WorkingLanguageId { get; set; }
        public AcademyVideoPagingFilteringModel PagingFilteringContext { get; set; }
        public List<AcademyVideoModel> AcademyVideos { get; set; }
    }
}

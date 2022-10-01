using System;
using System.Collections.Generic;
using WCore.Core.Domain.Academies;
using WCore.Framework.Models;

namespace WCore.Web.Models.Academies
{
    public class AcademyModel : BaseWCoreEntityModel
    {
        #region Ctor
        public AcademyModel()
        {
            AcademyImages = new AcademyImageListModel();
            AcademyFiles = new AcademyFileListModel();
            AcademyVideos = new AcademyVideoListModel();
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }

        public int AcademyCategoryId { get; set; }
        public AcademyCategoryModel AcademyCategory { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public string SeName { get; set; }

        public AcademyImageListModel AcademyImages { get; set; }
        public AcademyFileListModel AcademyFiles { get; set; }
        public AcademyVideoListModel AcademyVideos { get; set; }


        public PageTitleModel PageTitle { get; set; }
        #endregion
    }
    public class AcademyCategoryModel : BaseWCoreEntityModel
    {
        #region Ctor
        public AcademyCategoryModel()
        {
            Academies = new AcademyListModel();
            Parents = new AcademyCategoryListModel();
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Banner { get; set; }

        public int ParentId { get; set; }
        public AcademyCategoryModel Parent { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public string SeName { get; set; }

        public AcademyListModel Academies { get; set; }
        public AcademyCategoryListModel Parents { get; set; }
        public PageTitleModel PageTitle { get; set; }
        #endregion
    }
    public class AcademyImageModel : BaseWCoreEntityModel
    {
        #region Ctor
        public AcademyImageModel()
        {
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Big { get; set; }
        public string Original { get; set; }

        public int AcademyId { get; set; }
        public virtual AcademyModel Academy { get; set; }

        public int DisplayOrder { get; set; }
        #endregion
    }
    public class AcademyFileModel : BaseWCoreEntityModel
    {
        #region Ctor
        public AcademyFileModel()
        {
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Path { get; set; }

        public int AcademyId { get; set; }
        public virtual AcademyModel Academy { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        #endregion
    }
    public class AcademyVideoModel : BaseWCoreEntityModel
    {
        #region Ctor
        public AcademyVideoModel()
        {
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Path { get; set; }
        public AcademyVideoResource AcademyVideoResource { get; set; }

        public int AcademyId { get; set; }
        public virtual AcademyModel Academy { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        #endregion
    }
}

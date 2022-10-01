using System;
using WCore.Core;
using WCore.Core.Domain.Academies;

namespace WCore.Services.Academies
{
    public interface IAcademyService : IRepository<Academy>
    {
        IPagedList<Academy> GetAllByFilters(int? AcademyCategoryId = null,
            string Title = "",
            bool? IsArchived = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface IAcademyCategoryService : IRepository<AcademyCategory>
    {
        IPagedList<AcademyCategory> GetAllByFilters(int? ParentId = null,
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface IAcademyImageService : IRepository<AcademyImage>
    {
        IPagedList<AcademyImage> GetAllByFilters(int AcademyId,
            int Skip = 0, 
            int Take = int.MaxValue);
    }
    public interface IAcademyFileService : IRepository<AcademyFile>
    {
        IPagedList<AcademyFile> GetAllByFilters(int AcademyId,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface IAcademyVideoService : IRepository<AcademyVideo>
    {
        IPagedList<AcademyVideo> GetAllByFilters(int AcademyId,
            AcademyVideoResource? AcademyVideoResource = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
}

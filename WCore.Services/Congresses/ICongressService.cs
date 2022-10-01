using System;
using WCore.Core;
using WCore.Core.Domain.Congresses;

namespace WCore.Services.Congresses
{
    public interface ICongressService : IRepository<Congress>
    {
        IPagedList<Congress> GetAllByFilters(string Title = "",
            bool? IsArchived = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            DateTime? StartDate = null,
            DateTime? EndDate = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface ICongressPaperTypeService : IRepository<CongressPaperType>
    {
        IPagedList<CongressPaperType> GetAllByFilters(int? CongressId = null,
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface ICongressPaperService : IRepository<CongressPaper>
    {
        IPagedList<CongressPaper> GetAllByFilters(int? CongressPaperTypeId = null,
            int? CongressId = null,
            string Title = "",
            string Code = "",
            string OwnersName = "",
            string OwnersSurname = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface ICongressPresentationTypeService : IRepository<CongressPresentationType>
    {
        IPagedList<CongressPresentationType> GetAllByFilters(int? CongressId = null,
            string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface ICongressPresentationService : IRepository<CongressPresentation>
    {
        IPagedList<CongressPresentation> GetAllByFilters(int? CongressPresentationTypeId = null,
            int? CongressId = null,
            string Title = "",
            string Code = "",
            string OwnersName = "",
            string OwnersSurname = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface ICongressImageService : IRepository<CongressImage>
    {
        IPagedList<CongressImage> GetAllByFilters(int CongressId, int Skip = 0, int Take = int.MaxValue);
    }
}

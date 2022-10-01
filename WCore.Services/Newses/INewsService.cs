using System;
using WCore.Core;
using WCore.Core.Domain.Newses;

namespace WCore.Services.Newses
{
    public interface INewsService : IRepository<News>
    {
        IPagedList<News> GetAllByFilters(int? NewsCategoryId = null,
            string Title = "",
            bool? IsArchived = null,
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            bool? ShowOnHome = null,
            DateTime? StartDate = null,
            DateTime? EndDate = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface INewsCategoryService : IRepository<NewsCategory>
    {
        IPagedList<NewsCategory> GetAllByFilters(string Title = "",
            bool? IsActive = null,
            bool? Deleted = null,
            bool? ShowOn = null,
            int Skip = 0,
            int Take = int.MaxValue);
    }
    public interface INewsImageService : IRepository<NewsImage>
    {
        IPagedList<NewsImage> GetAllByFilters(int NewsId,
            int Skip = 0, 
            int Take = int.MaxValue);
    }
}

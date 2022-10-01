using System.Collections.Generic;
using WCore.Core.Domain.Catalog;

namespace WCore.Services.Catalog
{
    /// <summary>
    /// Category template service interface
    /// </summary>
    public partial interface ICategoryTemplateService : IRepository<CategoryTemplate>
    {
        /// <summary>
        /// Gets all category templates
        /// </summary>
        /// <returns>Category templates</returns>
        IList<CategoryTemplate> GetAllCategoryTemplates();
    }
}

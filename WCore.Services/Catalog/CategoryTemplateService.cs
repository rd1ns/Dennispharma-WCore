using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;

namespace WCore.Services.Catalog
{
    /// <summary>
    /// Category template service
    /// </summary>
    public partial class CategoryTemplateService : Repository<CategoryTemplate>, ICategoryTemplateService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<CategoryTemplate> _categoryTemplateRepository;

        #endregion

        #region Ctor

        public CategoryTemplateService(WCoreContext context, ICacheKeyService cacheKeyService,
        IEventPublisher eventPublisher,
            IRepository<CategoryTemplate> categoryTemplateRepository) : base(context)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _categoryTemplateRepository = categoryTemplateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all category templates
        /// </summary>
        /// <returns>Category templates</returns>
        public virtual IList<CategoryTemplate> GetAllCategoryTemplates()
        {
            var query = from pt in context.CategoryTemplates
                        orderby pt.DisplayOrder, pt.Id
                        select pt;

            var templates = query.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.CategoryTemplatesAllCacheKey));

            return templates;
        }

        #endregion
    }
}
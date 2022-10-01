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
    /// Product template service
    /// </summary>
    public partial class ProductTemplateService : Repository<ProductTemplate>, IProductTemplateService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ProductTemplate> _productTemplateRepository;

        #endregion

        #region Ctor

        public ProductTemplateService(WCoreContext context, ICacheKeyService cacheKeyService,
        IEventPublisher eventPublisher,
            IRepository<ProductTemplate> productTemplateRepository) : base(context)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _productTemplateRepository = productTemplateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void DeleteProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException(nameof(productTemplate));

            _productTemplateRepository.Delete(productTemplate);

            //event notification
            _eventPublisher.EntityDeleted(productTemplate);
        }

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product templates</returns>
        public virtual IList<ProductTemplate> GetAllProductTemplates()
        {
            var query = from pt in context.ProductTemplates
                        orderby pt.DisplayOrder, pt.Id
                        select pt;

            var templates = query.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductTemplatesAllCacheKey));

            return templates;
        }

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="productTemplateId">Product template identifier</param>
        /// <returns>Product template</returns>
        public virtual ProductTemplate GetProductTemplateById(int productTemplateId)
        {
            if (productTemplateId == 0)
                return null;

            return _productTemplateRepository.ToCachedGetById(productTemplateId);
        }

        /// <summary>
        /// Inserts product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void InsertProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException(nameof(productTemplate));

            _productTemplateRepository.Insert(productTemplate);

            //event notification
            _eventPublisher.EntityInserted(productTemplate);
        }

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void UpdateProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException(nameof(productTemplate));

            _productTemplateRepository.Update(productTemplate);

            //event notification
            _eventPublisher.EntityUpdated(productTemplate);
        }

        #endregion
    }
}
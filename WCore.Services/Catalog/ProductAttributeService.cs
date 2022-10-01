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
    /// Product attribute service
    /// </summary>
    public partial class ProductAttributeService : Repository<ProductAttribute>, IProductAttributeService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<PredefinedProductAttributeValue> _predefinedProductAttributeValueRepository;
        private readonly IRepository<ProductAttribute> _productAttributeRepository;
        private readonly IRepository<ProductAttributeCombination> _productAttributeCombinationRepository;
        private readonly IRepository<ProductProductAttribute> _productProductAttributeRepository;
        private readonly IRepository<ProductAttributeValue> _productAttributeValueRepository;

        #endregion

        #region Ctor

        public ProductAttributeService(WCoreContext context, ICacheKeyService cacheKeyService,
            IEventPublisher eventPublisher,
            IRepository<PredefinedProductAttributeValue> predefinedProductAttributeValueRepository,
            IRepository<ProductAttribute> productAttributeRepository,
            IRepository<ProductAttributeCombination> productAttributeCombinationRepository,
            IRepository<ProductProductAttribute> productProductAttributeRepository,
            IRepository<ProductAttributeValue> productAttributeValueRepository) : base(context)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _predefinedProductAttributeValueRepository = predefinedProductAttributeValueRepository;
            _productAttributeRepository = productAttributeRepository;
            _productAttributeCombinationRepository = productAttributeCombinationRepository;
            _productProductAttributeRepository = productProductAttributeRepository;
            _productAttributeValueRepository = productAttributeValueRepository;
        }

        #endregion

        #region Methods

        #region Product attributes

        /// <summary>
        /// Deletes a product attribute
        /// </summary>
        /// <param name="productAttribute">Product attribute</param>
        public virtual void DeleteProductAttribute(ProductAttribute productAttribute)
        {
            if (productAttribute == null)
                throw new ArgumentNullException(nameof(productAttribute));

            _productAttributeRepository.Delete(productAttribute);

            //event notification
            _eventPublisher.EntityDeleted(productAttribute);
        }

        /// <summary>
        /// Deletes product attributes
        /// </summary>
        /// <param name="productAttributes">Product attributes</param>
        public virtual void DeleteProductAttributes(IList<ProductAttribute> productAttributes)
        {
            if (productAttributes == null)
                throw new ArgumentNullException(nameof(productAttributes));

            foreach (var productAttribute in productAttributes)
            {
                DeleteProductAttribute(productAttribute);
            }
        }

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Product attributes</returns>
        public virtual IPagedList<ProductAttribute> GetAllProductAttributes(int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = from pa in context.ProductAttributes
                        orderby pa.Name
                        select pa;

            var productAttributes = new PagedList<ProductAttribute>(query, pageIndex, pageSize);

            return productAttributes;
        }

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <returns>Product attribute </returns>
        public virtual ProductAttribute GetProductAttributeById(int productAttributeId)
        {
            if (productAttributeId == 0)
                return null;

            return _productAttributeRepository.ToCachedGetById(productAttributeId);
        }

        /// <summary>
        /// Gets product attributes 
        /// </summary>
        /// <param name="productAttributeIds">Product attribute identifiers</param>
        /// <returns>Product attributes </returns>
        public virtual IList<ProductAttribute> GetProductAttributeByIds(int[] productAttributeIds)
        {
            if (productAttributeIds == null || productAttributeIds.Length == 0)
                return new List<ProductAttribute>();

            var query = from p in context.ProductAttributes
                        where productAttributeIds.Contains(p.Id)
                        select p;

            return query.ToList();
        }

        /// <summary>
        /// Inserts a product attribute
        /// </summary>
        /// <param name="productAttribute">Product attribute</param>
        public virtual void InsertProductAttribute(ProductAttribute productAttribute)
        {
            if (productAttribute == null)
                throw new ArgumentNullException(nameof(productAttribute));

            _productAttributeRepository.Insert(productAttribute);

            //event notification
            _eventPublisher.EntityInserted(productAttribute);
        }

        /// <summary>
        /// Updates the product attribute
        /// </summary>
        /// <param name="productAttribute">Product attribute</param>
        public virtual void UpdateProductAttribute(ProductAttribute productAttribute)
        {
            if (productAttribute == null)
                throw new ArgumentNullException(nameof(productAttribute));

            _productAttributeRepository.Update(productAttribute);

            //event notification
            _eventPublisher.EntityUpdated(productAttribute);
        }

        /// <summary>
        /// Returns a list of IDs of not existing attributes
        /// </summary>
        /// <param name="attributeId">The IDs of the attributes to check</param>
        /// <returns>List of IDs not existing attributes</returns>
        public virtual int[] GetNotExistingAttributes(int[] attributeId)
        {
            if (attributeId == null)
                throw new ArgumentNullException(nameof(attributeId));

            var query = context.ProductAttributes;
            var queryFilter = attributeId.Distinct().ToArray();
            var filter = query.Select(a => a.Id).Where(m => queryFilter.Contains(m)).ToList();
            return queryFilter.Except(filter).ToArray();
        }

        #endregion

        #region Product attributes mappings

        /// <summary>
        /// Deletes a product attribute mapping
        /// </summary>
        /// <param name="productProductAttribute">Product attribute mapping</param>
        public virtual void DeleteProductProductAttribute(ProductProductAttribute productProductAttribute)
        {
            if (productProductAttribute == null)
                throw new ArgumentNullException(nameof(productProductAttribute));

            _productProductAttributeRepository.Delete(productProductAttribute);

            //event notification
            _eventPublisher.EntityDeleted(productProductAttribute);
        }

        /// <summary>
        /// Gets product attribute mappings by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product attribute mapping collection</returns>
        public virtual IList<ProductProductAttribute> GetProductProductAttributesByProductId(int productId)
        {
            var allCacheKey = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductProductAttributesAllCacheKey, productId);

            var query = from pam in context.ProductProductAttributes
                        orderby pam.DisplayOrder, pam.Id
                        where pam.ProductId == productId
                        select pam;

            var attributes = query.ToCachedList(allCacheKey) ?? new List<ProductProductAttribute>();

            return attributes;
        }

        /// <summary>
        /// Gets a product attribute mapping
        /// </summary>
        /// <param name="productProductAttributeId">Product attribute mapping identifier</param>
        /// <returns>Product attribute mapping</returns>
        public virtual ProductProductAttribute GetProductProductAttributeById(int productProductAttributeId)
        {
            if (productProductAttributeId == 0)
                return null;

            return _productProductAttributeRepository.ToCachedGetById(productProductAttributeId);
        }

        /// <summary>
        /// Inserts a product attribute mapping
        /// </summary>
        /// <param name="productProductAttribute">The product attribute mapping</param>
        public virtual void InsertProductProductAttribute(ProductProductAttribute productProductAttribute)
        {
            if (productProductAttribute == null)
                throw new ArgumentNullException(nameof(productProductAttribute));

            _productProductAttributeRepository.Insert(productProductAttribute);

            //event notification
            _eventPublisher.EntityInserted(productProductAttribute);
        }

        /// <summary>
        /// Updates the product attribute mapping
        /// </summary>
        /// <param name="productProductAttribute">The product attribute mapping</param>
        public virtual void UpdateProductProductAttribute(ProductProductAttribute productProductAttribute)
        {
            if (productProductAttribute == null)
                throw new ArgumentNullException(nameof(productProductAttribute));

            _productProductAttributeRepository.Update(productProductAttribute);

            //event notification
            _eventPublisher.EntityUpdated(productProductAttribute);
        }

        #endregion

        #region Product attribute values

        /// <summary>
        /// Deletes a product attribute value
        /// </summary>
        /// <param name="productAttributeValue">Product attribute value</param>
        public virtual void DeleteProductAttributeValue(ProductAttributeValue productAttributeValue)
        {
            if (productAttributeValue == null)
                throw new ArgumentNullException(nameof(productAttributeValue));

            _productAttributeValueRepository.Delete(productAttributeValue);

            //event notification
            _eventPublisher.EntityDeleted(productAttributeValue);
        }

        /// <summary>
        /// Gets product attribute values by product attribute mapping identifier
        /// </summary>
        /// <param name="productProductAttributeId">The product attribute mapping identifier</param>
        /// <returns>Product attribute mapping collection</returns>
        public virtual IList<ProductAttributeValue> GetProductAttributeValues(int productProductAttributeId)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductAttributeValuesAllCacheKey, productProductAttributeId);

            var query = from pav in context.ProductAttributeValues
                        orderby pav.DisplayOrder, pav.Id
                        where pav.ProductProductAttributeId == productProductAttributeId
                        select pav;
            var productAttributeValues = query.ToCachedList(key);

            return productAttributeValues;
        }

        /// <summary>
        /// Gets a product attribute value
        /// </summary>
        /// <param name="productAttributeValueId">Product attribute value identifier</param>
        /// <returns>Product attribute value</returns>
        public virtual ProductAttributeValue GetProductAttributeValueById(int productAttributeValueId)
        {
            if (productAttributeValueId == 0)
                return null;

            return _productAttributeValueRepository.ToCachedGetById(productAttributeValueId);
        }

        /// <summary>
        /// Inserts a product attribute value
        /// </summary>
        /// <param name="productAttributeValue">The product attribute value</param>
        public virtual void InsertProductAttributeValue(ProductAttributeValue productAttributeValue)
        {
            if (productAttributeValue == null)
                throw new ArgumentNullException(nameof(productAttributeValue));

            _productAttributeValueRepository.Insert(productAttributeValue);

            //event notification
            _eventPublisher.EntityInserted(productAttributeValue);
        }

        /// <summary>
        /// Updates the product attribute value
        /// </summary>
        /// <param name="productAttributeValue">The product attribute value</param>
        public virtual void UpdateProductAttributeValue(ProductAttributeValue productAttributeValue)
        {
            if (productAttributeValue == null)
                throw new ArgumentNullException(nameof(productAttributeValue));

            _productAttributeValueRepository.Update(productAttributeValue);

            //event notification
            _eventPublisher.EntityUpdated(productAttributeValue);
        }

        #endregion

        #region Predefined product attribute values

        /// <summary>
        /// Deletes a predefined product attribute value
        /// </summary>
        /// <param name="ppav">Predefined product attribute value</param>
        public virtual void DeletePredefinedProductAttributeValue(PredefinedProductAttributeValue ppav)
        {
            if (ppav == null)
                throw new ArgumentNullException(nameof(ppav));

            _predefinedProductAttributeValueRepository.Delete(ppav);

            //event notification
            _eventPublisher.EntityDeleted(ppav);
        }

        /// <summary>
        /// Gets predefined product attribute values by product attribute identifier
        /// </summary>
        /// <param name="productAttributeId">The product attribute identifier</param>
        /// <returns>Product attribute mapping collection</returns>
        public virtual IList<PredefinedProductAttributeValue> GetPredefinedProductAttributeValues(int productAttributeId)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.PredefinedProductAttributeValuesAllCacheKey, productAttributeId);

            var query = from ppav in context.PredefinedProductAttributeValues
                        orderby ppav.DisplayOrder, ppav.Id
                        where ppav.ProductAttributeId == productAttributeId
                        select ppav;

            var values = query.ToCachedList(key);

            return values;
        }

        /// <summary>
        /// Gets a predefined product attribute value
        /// </summary>
        /// <param name="id">Predefined product attribute value identifier</param>
        /// <returns>Predefined product attribute value</returns>
        public virtual PredefinedProductAttributeValue GetPredefinedProductAttributeValueById(int id)
        {
            if (id == 0)
                return null;

            return _predefinedProductAttributeValueRepository.ToCachedGetById(id);
        }

        /// <summary>
        /// Inserts a predefined product attribute value
        /// </summary>
        /// <param name="ppav">The predefined product attribute value</param>
        public virtual void InsertPredefinedProductAttributeValue(PredefinedProductAttributeValue ppav)
        {
            if (ppav == null)
                throw new ArgumentNullException(nameof(ppav));

            _predefinedProductAttributeValueRepository.Insert(ppav);

            //event notification
            _eventPublisher.EntityInserted(ppav);
        }

        /// <summary>
        /// Updates the predefined product attribute value
        /// </summary>
        /// <param name="ppav">The predefined product attribute value</param>
        public virtual void UpdatePredefinedProductAttributeValue(PredefinedProductAttributeValue ppav)
        {
            if (ppav == null)
                throw new ArgumentNullException(nameof(ppav));

            _predefinedProductAttributeValueRepository.Update(ppav);

            //event notification
            _eventPublisher.EntityUpdated(ppav);
        }

        #endregion

        #region Product attribute combinations

        /// <summary>
        /// Deletes a product attribute combination
        /// </summary>
        /// <param name="combination">Product attribute combination</param>
        public virtual void DeleteProductAttributeCombination(ProductAttributeCombination combination)
        {
            if (combination == null)
                throw new ArgumentNullException(nameof(combination));

            _productAttributeCombinationRepository.Delete(combination);

            //event notification
            _eventPublisher.EntityDeleted(combination);
        }

        /// <summary>
        /// Gets all product attribute combinations
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product attribute combinations</returns>
        public virtual IList<ProductAttributeCombination> GetAllProductAttributeCombinations(int productId)
        {
            if (productId == 0)
                return new List<ProductAttributeCombination>();

            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductAttributeCombinationsAllCacheKey, productId);

            var query = from c in context.ProductAttributeCombinations
                        orderby c.Id
                        where c.ProductId == productId
                        select c;
            var combinations = query.ToCachedList(key);

            return combinations;
        }

        /// <summary>
        /// Gets a product attribute combination
        /// </summary>
        /// <param name="productAttributeCombinationId">Product attribute combination identifier</param>
        /// <returns>Product attribute combination</returns>
        public virtual ProductAttributeCombination GetProductAttributeCombinationById(int productAttributeCombinationId)
        {
            if (productAttributeCombinationId == 0)
                return null;

            return _productAttributeCombinationRepository.ToCachedGetById(productAttributeCombinationId);
        }

        /// <summary>
        /// Gets a product attribute combination by SKU
        /// </summary>
        /// <param name="sku">SKU</param>
        /// <returns>Product attribute combination</returns>
        public virtual ProductAttributeCombination GetProductAttributeCombinationBySku(string sku)
        {
            if (string.IsNullOrEmpty(sku))
                return null;

            sku = sku.Trim();

            var query = from pac in context.ProductAttributeCombinations
                        orderby pac.Id
                        where pac.Sku == sku
                        select pac;
            var combination = query.FirstOrDefault();
            return combination;
        }

        /// <summary>
        /// Inserts a product attribute combination
        /// </summary>
        /// <param name="combination">Product attribute combination</param>
        public virtual void InsertProductAttributeCombination(ProductAttributeCombination combination)
        {
            if (combination == null)
                throw new ArgumentNullException(nameof(combination));

            _productAttributeCombinationRepository.Insert(combination);

            //event notification
            _eventPublisher.EntityInserted(combination);
        }

        /// <summary>
        /// Updates a product attribute combination
        /// </summary>
        /// <param name="combination">Product attribute combination</param>
        public virtual void UpdateProductAttributeCombination(ProductAttributeCombination combination)
        {
            if (combination == null)
                throw new ArgumentNullException(nameof(combination));

            _productAttributeCombinationRepository.Update(combination);

            //event notification
            _eventPublisher.EntityUpdated(combination);
        }

        #endregion

        #endregion
    }
}
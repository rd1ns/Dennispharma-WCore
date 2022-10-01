using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain;
using WCore.Core.Domain.Catalog;
using WCore.Services.Caching;
using WCore.Services.Caching.Extensions;
using WCore.Services.Events;
using WCore.Services.Seo;
using WCore.Services.Users;

namespace WCore.Services.Catalog
{
    /// <summary>
    /// Product tag service
    /// </summary>
    public partial class ProductTagService : Repository<ProductTag>, IProductTagService
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly ICacheKeyService _cacheKeyService;
        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<ProductProductTag> _productProductTagRepository;
        private readonly IRepository<ProductTag> _productTagRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ProductTagService(WCoreContext context, CatalogSettings catalogSettings,
            ICacheKeyService cacheKeyService,
            IUserService userService,
            IEventPublisher eventPublisher,
            IRepository<ProductProductTag> productProductTagMappingRepository,
            IRepository<ProductTag> productTagRepository,
            IStaticCacheManager staticCacheManager,
            IUrlRecordService urlRecordService,
            IWorkContext workContext) : base(context)
        {
            _catalogSettings = catalogSettings;
            _cacheKeyService = cacheKeyService;
            _userService = userService;
            _eventPublisher = eventPublisher;
            _productProductTagRepository = productProductTagMappingRepository;
            _productTagRepository = productTagRepository;
            _staticCacheManager = staticCacheManager;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Delete a product-product tag mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="productTagId">Product tag identifier</param>
        public virtual void DeleteProductProductTag(int productId, int productTagId)
        {
            var mappitngRecord = _productProductTagRepository.GetAll().FirstOrDefault(pptm => pptm.ProductId == productId && pptm.ProductTagId == productTagId);

            if (mappitngRecord is null)
                throw new Exception("Mppaing record not found");

            _productProductTagRepository.Delete(mappitngRecord);

            //event notification
            _eventPublisher.EntityDeleted(mappitngRecord);
        }

        /// <summary>
        /// Get product count for each of existing product tag
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Dictionary of "product tag ID : product count"</returns>
        private Dictionary<int, int> GetProductCount(int storeId, bool showHidden)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductTagCountCacheKey, storeId,
                _workContext.CurrentUser.UserType,
                showHidden);

            return _staticCacheManager.Get(key, () =>
            {
                //prepare input parameters
                var pStoreId = SqlParameterHelper.GetInt32Parameter("StoreId", storeId);

                //invoke stored procedure
                return context.Set<ProductTagWithCount>().FromSqlRaw("ProductTagCountLoadAll",
                        pStoreId, 0)
                    .ToDictionary(item => item.ProductTagId, item => item.ProductCount);
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        public virtual void DeleteProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException(nameof(productTag));

            _productTagRepository.Delete(productTag);

            //event notification
            _eventPublisher.EntityDeleted(productTag);
        }

        /// <summary>
        /// Delete product tags
        /// </summary>
        /// <param name="productTags">Product tags</param>
        public virtual void DeleteProductTags(IList<ProductTag> productTags)
        {
            if (productTags == null)
                throw new ArgumentNullException(nameof(productTags));

            foreach (var productTag in productTags)
            {
                DeleteProductTag(productTag);
            }
        }

        /// <summary>
        /// Gets all product tags
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Product tags</returns>
        public virtual IList<ProductTag> GetAllProductTags(string tagName = null)
        {
            var query = context.ProductTags;

            var allProductTags = query.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductTagAllCacheKey));

            if (!string.IsNullOrEmpty(tagName))
            {
                allProductTags = allProductTags.Where(tag => tag.Name.Contains(tagName)).ToList();
            }

            return allProductTags;
        }

        /// <summary>
        /// Gets all product tags by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product tags</returns>
        public virtual IList<ProductTag> GetAllProductTagsByProductId(int productId)
        {
            var key = _cacheKeyService.PrepareKeyForDefaultCache(WCoreCatalogDefaults.ProductTagAllByProductIdCacheKey, productId);

            var query = from pt in context.ProductTags
                        join ppt in context.ProductProductTags on pt.Id equals ppt.ProductTagId
                        where ppt.ProductId == productId
                        orderby pt.Id
                        select pt;

            var productTags = query.ToCachedList(key);

            return productTags;
        }

        /// <summary>
        /// Gets product tag
        /// </summary>
        /// <param name="productTagId">Product tag identifier</param>
        /// <returns>Product tag</returns>
        public virtual ProductTag GetProductTagById(int productTagId)
        {
            if (productTagId == 0)
                return null;

            return _productTagRepository.ToCachedGetById(productTagId);
        }

        /// <summary>
        /// Gets product tags
        /// </summary>
        /// <param name="productTagIds">Product tags identifiers</param>
        /// <returns>Product tags</returns>
        public virtual IList<ProductTag> GetProductTagsByIds(int[] productTagIds)
        {
            if (productTagIds == null || productTagIds.Length == 0)
                return new List<ProductTag>();

            var query = from p in context.ProductTags
                        where productTagIds.Contains(p.Id)
                        select p;

            return query.ToList();
        }

        /// <summary>
        /// Gets product tag by name
        /// </summary>
        /// <param name="name">Product tag name</param>
        /// <returns>Product tag</returns>
        public virtual ProductTag GetProductTagByName(string name)
        {
            var query = from pt in context.ProductTags
                        where pt.Name == name
                        select pt;

            var productTag = query.FirstOrDefault();
            return productTag;
        }

        /// <summary>
        /// Inserts a product-product tag mapping
        /// </summary>
        /// <param name="tagMapping">Product-product tag mapping</param>
        public virtual void InsertProductProductTag(ProductProductTag tagMapping)
        {
            if (tagMapping is null)
                throw new ArgumentNullException(nameof(tagMapping));

            _productProductTagRepository.Insert(tagMapping);

            //event notification
            _eventPublisher.EntityInserted(tagMapping);
        }

        /// <summary>
        /// Inserts a product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        public virtual void InsertProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException(nameof(productTag));

            _productTagRepository.Insert(productTag);

            //event notification
            _eventPublisher.EntityInserted(productTag);
        }

        /// <summary>
        /// Indicates whether a product tag exists
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="productTagId">Product tag identifier</param>
        /// <returns>Result</returns>
        public virtual bool ProductTagExists(Product product, int productTagId)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            return context.ProductProductTags.Any(pptm => pptm.ProductId == product.Id && pptm.ProductTagId == productTagId);
        }

        /// <summary>
        /// Updates the product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        public virtual void UpdateProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException(nameof(productTag));

            _productTagRepository.Update(productTag);

            var seName = _urlRecordService.ValidateSeName(productTag, string.Empty, productTag.Name, true);
            _urlRecordService.SaveSlug(productTag, seName, 0);

            //event notification
            _eventPublisher.EntityUpdated(productTag);
        }

        /// <summary>
        /// Get number of products
        /// </summary>
        /// <param name="productTagId">Product tag identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Number of products</returns>
        public virtual int GetProductCount(int productTagId, int storeId, bool showHidden = false)
        {
            var dictionary = GetProductCount(storeId, showHidden);
            if (dictionary.ContainsKey(productTagId))
                return dictionary[productTagId];

            return 0;
        }

        /// <summary>
        /// Update product tags
        /// </summary>
        /// <param name="product">Product for update</param>
        /// <param name="productTags">Product tags</param>
        public virtual void UpdateProductTags(Product product, string[] productTags)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //product tags
            var existingProductTags = GetAllProductTagsByProductId(product.Id);
            var productTagsToRemove = new List<ProductTag>();
            foreach (var existingProductTag in existingProductTags)
            {
                var found = false;
                foreach (var newProductTag in productTags)
                {
                    if (!existingProductTag.Name.Equals(newProductTag, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    found = true;
                    break;
                }

                if (!found)
                {
                    productTagsToRemove.Add(existingProductTag);
                }
            }

            foreach (var productTag in productTagsToRemove)
            {
                DeleteProductProductTag(product.Id, productTag.Id);
            }

            foreach (var productTagName in productTags)
            {
                ProductTag productTag;
                var productTag2 = GetProductTagByName(productTagName);
                if (productTag2 == null)
                {
                    //add new product tag
                    productTag = new ProductTag
                    {
                        Name = productTagName
                    };
                    InsertProductTag(productTag);
                }
                else
                {
                    productTag = productTag2;
                }

                if (!ProductTagExists(product, productTag.Id))
                {
                    InsertProductProductTag(new ProductProductTag { ProductTagId = productTag.Id, ProductId = product.Id });
                }

                var seName = _urlRecordService.ValidateSeName(productTag, string.Empty, productTag.Name, true);
                _urlRecordService.SaveSlug(productTag, seName, 0);
            }

            //cache
            _staticCacheManager.RemoveByPrefix(WCoreCatalogDefaults.ProductTagPrefixCacheKey);
        }

        #endregion

        #region MyRegion

        protected partial class ProductTagWithCount : BaseEntity
        {
            /// <summary>
            /// Gets or sets the product tag ID
            /// </summary>
            public int ProductTagId { get; set; }

            /// <summary>
            /// Gets or sets the count
            /// </summary>
            public int ProductCount { get; set; }
        }

        #endregion
    }
}
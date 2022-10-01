using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Discounts;
using WCore.Framework.Filters;
using WCore.Framework.Mvc;
using WCore.Services.Caching;
using WCore.Services.Catalog;
using WCore.Services.Discounts;
using WCore.Services.Localization;
using WCore.Services.Logging;
using WCore.Services.Messages;
using WCore.Services.Seo;
using WCore.Services.Stores;
using WCore.Services.Users;
using WCore.Web.Areas.Admin.Factories;
using WCore.Web.Areas.Admin.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Catalog;

namespace WCore.Web.Areas.Admin.Controllers
{
    public partial class CategoryController : BaseAdminController
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly ICategoryModelFactory _categoryModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IDiscountService _discountService;
        //private readonly IExportManager _exportManager;
        //private readonly IImportManager _importManager;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly INotificationService _notificationService;
        //private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public CategoryController(ICacheKeyService cacheKeyService,
            ICategoryModelFactory categoryModelFactory,
            ICategoryService categoryService,
            IUserActivityService userActivityService,
            IUserService userService,
            IDiscountService discountService,
            //IExportManager exportManager,
            //IImportManager importManager,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            INotificationService notificationService,
            //IPictureService pictureService,
            IProductService productService,
            IStaticCacheManager staticCacheManager,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext)
        {
            _cacheKeyService = cacheKeyService;
            _categoryModelFactory = categoryModelFactory;
            _categoryService = categoryService;
            _userActivityService = userActivityService;
            _userService = userService;
            _discountService = discountService;
            //_exportManager = exportManager;
            //_importManager = importManager;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _notificationService = notificationService;
            //_pictureService = pictureService;
            _productService = productService;
            _staticCacheManager = staticCacheManager;
            _storeMappingService = storeMappingService;
            _storeService = storeService;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateLocales(Category category, CategoryModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(category,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(category, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(category, seName, localized.LanguageId);
            }
        }

        protected virtual void UpdatePictureSeoNames(Category category)
        {
            //var picture = _pictureService.GetPictureById(category.PictureId);
            //if (picture != null)
            //    _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(category.Name));
        }

        protected virtual void SaveStoreMappings(Category category, CategoryModel model)
        {
            category.LimitedToStores = model.SelectedStoreIds.Any();
            _categoryService.UpdateCategory(category);

            var existingStoreMappings = _storeMappingService.GetStoreMappings(category);
            var allStores = _storeService.GetAllByFilters();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(category, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }

        #endregion

        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //prepare model
            var model = _categoryModelFactory.PrepareCategorySearchModel(new CategorySearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(CategorySearchModel searchModel)
        {
            //prepare model
            var model = _categoryModelFactory.PrepareCategoryListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {
            //prepare model
            var model = _categoryModelFactory.PrepareCategoryModel(new CategoryModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(CategoryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var category = model.ToEntity<Category>();
                category.CreatedOn = DateTime.Now;
                category.UpdatedOn = DateTime.Now;
                _categoryService.InsertCategory(category);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name, true);
                _urlRecordService.SaveSlug(category, model.SeName, 0);

                //locales
                UpdateLocales(category, model);

                //discounts
                var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToCategories, showHidden: true);
                foreach (var discount in allDiscounts)
                {
                    if (model.SelectedDiscountIds != null && model.SelectedDiscountIds.Contains(discount.Id))
                        _categoryService.InsertDiscountAppliedToCategory(new DiscountAppliedToCategory { DiscountId = discount.Id, EntityId = category.Id });
                }

                _categoryService.UpdateCategory(category);

                //update picture seo file name
                UpdatePictureSeoNames(category);

                //stores
                SaveStoreMappings(category, model);

                //activity log
                _userActivityService.InsertActivity("AddNewCategory",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewCategory"), category.Name), category);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = category.Id });
            }

            //prepare model
            model = _categoryModelFactory.PrepareCategoryModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            //try to get a category with the specified id
            var category = _categoryService.GetCategoryById(id);
            if (category == null || category.Deleted)
                return RedirectToAction("List");

            //prepare model
            var model = _categoryModelFactory.PrepareCategoryModel(null, category);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(CategoryModel model, bool continueEditing)
        {
            //try to get a category with the specified id
            var category = _categoryService.GetCategoryById(model.Id);
            if (category == null || category.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var prevPictureId = category.PictureId;

                //if parent category changes, we need to clear cache for previous parent category
                if (category.ParentCategoryId != model.ParentCategoryId)
                {
                    var prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.CategoriesByParentCategoryPrefixCacheKey, category.ParentCategoryId);
                    _staticCacheManager.RemoveByPrefix(prefix);
                    prefix = _cacheKeyService.PrepareKeyPrefix(WCoreCatalogDefaults.CategoriesChildIdentifiersPrefixCacheKey, category.ParentCategoryId);
                    _staticCacheManager.RemoveByPrefix(prefix);
                }

                category = model.ToEntity(category);
                category.UpdatedOn = DateTime.Now;
                _categoryService.UpdateCategory(category);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name, true);
                _urlRecordService.SaveSlug(category, model.SeName, 0);

                //locales
                UpdateLocales(category, model);

                //discounts
                var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToCategories, showHidden: true);
                foreach (var discount in allDiscounts)
                {
                    if (model.SelectedDiscountIds != null && model.SelectedDiscountIds.Contains(discount.Id))
                    {
                        //new discount
                        if (_categoryService.GetDiscountAppliedToCategory(category.Id, discount.Id) is null)
                            _categoryService.InsertDiscountAppliedToCategory(new DiscountAppliedToCategory { DiscountId = discount.Id, EntityId = category.Id });
                    }
                    else
                    {
                        //remove discount
                        if (_categoryService.GetDiscountAppliedToCategory(category.Id, discount.Id) is DiscountAppliedToCategory mapping)
                            _categoryService.DeleteDiscountAppliedToCategory(mapping);
                    }
                }

                _categoryService.UpdateCategory(category);

                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != category.PictureId)
                {
                    //var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    //if (prevPicture != null)
                    //    _pictureService.DeletePicture(prevPicture);
                }

                //update picture seo file name
                UpdatePictureSeoNames(category);

                //stores
                SaveStoreMappings(category, model);

                //activity log
                _userActivityService.InsertActivity("EditCategory",
                    string.Format(_localizationService.GetResource("ActivityLog.EditCategory"), category.Name), category);

                _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = category.Id });
            }

            //prepare model
            model = _categoryModelFactory.PrepareCategoryModel(model, category, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //try to get a category with the specified id
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return RedirectToAction("List");

            _categoryService.DeleteCategory(category);

            //activity log
            _userActivityService.InsertActivity("DeleteCategory",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteCategory"), category.Name), category);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                _categoryService.DeleteCategories(_categoryService.GetCategoriesByIds(selectedIds.ToArray()).Where(p => _workContext.CurrentVendor == null).ToList());
            }

            return Json(new { Result = true });
        }

        #endregion

        #region Export / Import

        public virtual IActionResult ExportXml()
        {
            try
            {
                //var xml = _exportManager.ExportCategoriesToXml();

                //return File(Encoding.UTF8.GetBytes(xml), "application/xml", "categories.xml");
                return null;
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        public virtual IActionResult ExportXlsx()
        {
            try
            {
                //var bytes = _exportManager
                //    .ExportCategoriesToXlsx(_categoryService.GetAllCategories(showHidden: true).ToList());

                //return File(bytes, MimeTypes.TextXlsx, "categories.xlsx");
                return null;
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ImportFromXlsx(IFormFile importexcelfile)
        {
            //a vendor cannot import categories
            if (_workContext.CurrentVendor != null)
                return null;

            try
            {
                //if (importexcelfile != null && importexcelfile.Length > 0)
                //{
                //    _importManager.ImportCategoriesFromXlsx(importexcelfile.OpenReadStream());
                //}
                //else
                //{
                //    _notificationService.ErrorNotification(_localizationService.GetResource("Admin.Common.UploadFile"));
                //    return RedirectToAction("List");
                //}

                //_notificationService.SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Imported"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        #endregion

        #region Products

        [HttpPost]
        public virtual IActionResult ProductList(CategoryProductSearchModel searchModel)
        {
            //try to get a category with the specified id
            var category = _categoryService.GetCategoryById(searchModel.CategoryId)
                ?? throw new ArgumentException("No category found with the specified id");

            //prepare model
            var model = _categoryModelFactory.PrepareCategoryProductListModel(searchModel, category);

            return Json(model);
        }

        public virtual IActionResult ProductUpdate(CategoryProductModel model)
        {
            //try to get a product category with the specified id
            var productCategory = _categoryService.GetProductCategoryById(model.Id)
                ?? throw new ArgumentException("No product category mapping found with the specified id");

            //fill entity from product
            productCategory = model.ToEntity(productCategory);
            _categoryService.UpdateProductCategory(productCategory);

            return new NullJsonResult();
        }

        public virtual IActionResult ProductDelete(int id)
        {
            //try to get a product category with the specified id
            var productCategory = _categoryService.GetProductCategoryById(id)
                ?? throw new ArgumentException("No product category mapping found with the specified id", nameof(id));

            _categoryService.DeleteProductCategory(productCategory);

            return new NullJsonResult();
        }

        public virtual IActionResult ProductAddPopup(int categoryId)
        {
            //prepare model
            var model = _categoryModelFactory.PrepareAddProductToCategorySearchModel(new AddProductToCategorySearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ProductAddPopupList(AddProductToCategorySearchModel searchModel)
        {
            //prepare model
            var model = _categoryModelFactory.PrepareAddProductToCategoryListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        //[FormValueRequired("save")]
        public virtual IActionResult ProductAddPopup(AddProductToCategoryModel model)
        {
            //get selected products
            var selectedProducts = _productService.GetProductsByIds(model.SelectedProductIds.ToArray());
            if (selectedProducts.Any())
            {
                var existingProductCategories = _categoryService.GetProductCategoriesByCategoryId(model.CategoryId, showHidden: true);
                foreach (var product in selectedProducts)
                {
                    //whether product category with such parameters already exists
                    if (_categoryService.FindProductCategory(existingProductCategories, product.Id, model.CategoryId) != null)
                        continue;

                    //insert the new product category mapping
                    _categoryService.InsertProductCategory(new ProductCategory
                    {
                        CategoryId = model.CategoryId,
                        ProductId = product.Id,
                        IsFeaturedProduct = false,
                        DisplayOrder = 1
                    });
                }
            }

            ViewBag.RefreshPage = true;

            return View(new AddProductToCategorySearchModel());
        }

        #endregion
    }
}

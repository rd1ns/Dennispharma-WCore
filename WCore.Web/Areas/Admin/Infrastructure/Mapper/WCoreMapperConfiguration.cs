using AutoMapper;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Congresses;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Logging;
using WCore.Core.Domain.Newses;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Popup;
using WCore.Core.Domain.Roles;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Stores;
using WCore.Core.Domain.Teams;
using WCore.Core.Domain.Templates;
using WCore.Core.Domain.Users;
using WCore.Core.Domain.Vendors;
using WCore.Core.Infrastructure.Mapper;
using WCore.Web.Areas.Admin.Models.Academies;
using WCore.Web.Areas.Admin.Models.Catalog;
using WCore.Web.Areas.Admin.Models.Common;
using WCore.Web.Areas.Admin.Models.Congresses;
using WCore.Web.Areas.Admin.Models.Directory;
using WCore.Web.Areas.Admin.Models.Discounts;
using WCore.Web.Areas.Admin.Models.DynamicForms;
using WCore.Web.Areas.Admin.Models.Galleries;
using WCore.Web.Areas.Admin.Models.Localization;
using WCore.Web.Areas.Admin.Models.Logs;
using WCore.Web.Areas.Admin.Models.Newses;
using WCore.Web.Areas.Admin.Models.Pages;
using WCore.Web.Areas.Admin.Models.Popups;
using WCore.Web.Areas.Admin.Models.Roles;
using WCore.Web.Areas.Admin.Models.Settings;
using WCore.Web.Areas.Admin.Models.Stores;
using WCore.Web.Areas.Admin.Models.Teams;
using WCore.Web.Areas.Admin.Models.Templates;
using WCore.Web.Areas.Admin.Models.UserAgencies;
using WCore.Web.Areas.Admin.Models.Users;
using WCore.Web.Areas.Admin.Models.Vendors;

namespace WCore.Web.Areas.Admin.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class WCoreMapperConfiguration : Profile, IOrderedMapperProfile
    {

        public WCoreMapperConfiguration()
        {
            CreateCatalogMaps();

            CreateMap<Log, LogModel>();
            CreateMap<LogModel, Log>();

            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<UserRegistrationForm, UserRegistrationFormModel>();
            CreateMap<UserRegistrationFormModel, UserRegistrationForm>();

            CreateMap<UserAgency, UserAgencyModel>();
            CreateMap<UserAgencyModel, UserAgency>();

            CreateMap<UserAgencyAuthorization, UserAgencyAuthorizationModel>();
            CreateMap<UserAgencyAuthorizationModel, UserAgencyAuthorization>();

            CreateMap<Language, LanguageModel>();
            CreateMap<LanguageModel, Language>();

            CreateMap<LocaleStringResource, LocaleResourceModel>();
            CreateMap<LocaleResourceModel, LocaleStringResource>();

            CreateMap<Currency, CurrencyModel>();
            CreateMap<CurrencyModel, Currency>();

            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            CreateMap<City, CityModel>();
            CreateMap<CityModel, City>();


            CreateMap<SeoSettings, SeoSettingsModel>();
            CreateMap<SeoSettingsModel, SeoSettings>();

            CreateMap<CommonSettings, CommonSettingsModel>();
            CreateMap<CommonSettingsModel, CommonSettings>();

            CreateMap<MailSettings, MailSettingsModel>();
            CreateMap<MailSettingsModel, MailSettings>();

            CreateMap<NotificationSettings, NotificationSettingsModel>();
            CreateMap<NotificationSettingsModel, NotificationSettings>();

            CreateMap<Template, TemplateModel>();
            CreateMap<TemplateModel, Template>();

            CreateMap<Menu, MenuModel>();
            CreateMap<MenuModel, Menu>();

            CreateMap<Role, RoleModel>();
            CreateMap<RoleModel, Role>();

            CreateMap<RoleGroup, RoleGroupModel>();
            CreateMap<RoleGroupModel, RoleGroup>();

            CreateMap<Page, PageModel>();
            CreateMap<PageModel, Page>();

            CreateMap<Gallery, GalleryModel>();
            CreateMap<GalleryModel, Gallery>();

            CreateMap<GalleryImage, GalleryImageModel>();
            CreateMap<GalleryImageModel, GalleryImage>();

            CreateMap<DynamicForm, DynamicFormModel>();
            CreateMap<DynamicFormModel, DynamicForm>();

            CreateMap<DynamicFormRecord, DynamicFormRecordModel>();
            CreateMap<DynamicFormRecordModel, DynamicFormRecord>();

            CreateMap<DynamicFormElement, DynamicFormElementModel>();
            CreateMap<DynamicFormElementModel, DynamicFormElement>();

            //Vendor

            CreateMap<VendorAttribute, VendorAttributeModel>();
            CreateMap<VendorAttributeModel, VendorAttribute>();

            CreateMap<VendorAttributeValue, VendorAttributeValueModel>();
            CreateMap<VendorAttributeValueModel, VendorAttributeValue>();

            CreateMap<Vendor, VendorModel>();
            CreateMap<VendorModel, Vendor>();

            CreateMap<VendorNote, VendorNoteModel>();
            CreateMap<VendorNoteModel, VendorNote>();

            //Store
            CreateMap<Store, StoreModel>();
            CreateMap<StoreModel, Store>();

            //Congress
            CreateMap<CongressImage, CongressImageModel>();
            CreateMap<CongressImageModel, CongressImage>();

            CreateMap<Congress, CongressModel>();
            CreateMap<CongressModel, Congress>();

            CreateMap<CongressPaper, CongressPaperModel>();
            CreateMap<CongressPaperModel, CongressPaper>();

            CreateMap<CongressPaperType, CongressPaperTypeModel>();
            CreateMap<CongressPaperTypeModel, CongressPaperType>();

            CreateMap<CongressPresentation, CongressPresentationModel>();
            CreateMap<CongressPresentationModel, CongressPresentation>();

            CreateMap<CongressPresentationType, CongressPresentationTypeModel>();
            CreateMap<CongressPresentationTypeModel, CongressPresentationType>();

            //News
            CreateMap<NewsImage, NewsImageModel>();
            CreateMap<NewsImageModel, NewsImage>();

            CreateMap<News, NewsModel>();
            CreateMap<NewsModel, News>();

            CreateMap<NewsCategory, NewsCategoryModel>();
            CreateMap<NewsCategoryModel, NewsCategory>();

            //Team
            CreateMap<Team, TeamModel>();
            CreateMap<TeamModel, Team>();

            CreateMap<TeamCategory, TeamCategoryModel>();
            CreateMap<TeamCategoryModel, TeamCategory>();

            //Academy
            CreateMap<Academy, AcademyModel>();
            CreateMap<AcademyModel, Academy>();

            CreateMap<AcademyCategory, AcademyCategoryModel>();
            CreateMap<AcademyCategoryModel, AcademyCategory>();

            CreateMap<AcademyImage, AcademyImageModel>();
            CreateMap<AcademyImageModel, AcademyImage>();

            CreateMap<AcademyFile, AcademyFileModel>();
            CreateMap<AcademyFileModel, AcademyFile>();

            CreateMap<AcademyVideo, AcademyVideoModel>();
            CreateMap<AcademyVideoModel, AcademyVideo>();

            //Popup
            CreateMap<Popup, PopupModel>();
            CreateMap<PopupModel, Popup>();


        }

        #region Utilities

        /// <summary>
        /// Create catalog maps 
        /// </summary>
        protected virtual void CreateCatalogMaps()
        {
            CreateMap<CatalogSettings, CatalogSettingsModel>()
                .ForMember(model => model.AllowAnonymousUsersToEmailAFriend_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.AllowAnonymousUsersToReviewProduct_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.AllowProductSorting_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.AllowProductViewModeChanging_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.AllowViewUnpublishedProductPage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.AvailableViewModes, options => options.Ignore())
                .ForMember(model => model.CategoryBreadcrumbEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.CompareProductsEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DefaultViewMode_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayDiscontinuedMessageForUnpublishedProducts_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayTaxShippingInfoFooter_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayTaxShippingInfoOrderDetailsPage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayTaxShippingInfoProductBoxes_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayTaxShippingInfoProductDetailsPage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayTaxShippingInfoShoppingCart_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayTaxShippingInfoWishlist_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.EmailAFriendEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportAllowDownloadImages_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportCategoriesUsingCategoryName_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportProductAttributes_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportProductCategoryBreadcrumb_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportProductSpecificationAttributes_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportRelatedEntitiesByName_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportProductUseLimitedToStores_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ExportImportSplitProductsFile_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.IncludeFullDescriptionInCompareProducts_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.IncludeShortDescriptionInCompareProducts_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ManufacturersBlockItemsToDisplay_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.NewProductsEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.NewProductsNumber_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.NotifyUserAboutProductReviewReply_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.NotifyStoreOwnerAboutNewProductReviews_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.NumberOfBestsellersOnHomepage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.NumberOfProductTags_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.PageShareCode_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductReviewPossibleOnlyAfterPurchasing_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductReviewsMustBeApproved_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductReviewsPageSizeOnAccountPage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductReviewsSortByCreatedDateAscending_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductsAlsoPurchasedEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductsAlsoPurchasedNumber_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductsByTagAllowUsersToSelectPageSize_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductsByTagPageSizeOptions_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductsByTagPageSize_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductSearchAutoCompleteEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductSearchEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductSearchAutoCompleteNumberOfProducts_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ProductSearchTermMinimumLength_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.RecentlyViewedProductsEnabled_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.RecentlyViewedProductsNumber_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.RemoveRequiredProducts_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.SearchPageAllowUsersToSelectPageSize_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.SearchPagePageSizeOptions_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.SearchPageProductsPerPage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowBestsellersOnHomepage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowCategoryProductNumberIncludingSubcategories_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowCategoryProductNumber_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowFreeShippingNotification_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowGtin_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowLinkToAllResultInSearchAutoComplete_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowManufacturerPartNumber_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowProductImagesInSearchAutoComplete_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowProductReviewsOnAccountPage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowProductReviewsPerStore_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowProductsFromSubcategories_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowShareButton_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowSkuOnCatalogPages_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.ShowSkuOnProductDetailsPage_OverrideForStore, options => options.Ignore())
                .ForMember(model => model.DisplayDatePreOrderAvailability_OverrideForStore, mo => mo.Ignore())
                .ForMember(model => model.SortOptionSearchModel, options => options.Ignore())
                .ForMember(model => model.ReviewTypeSearchModel, options => options.Ignore());
            CreateMap<CatalogSettingsModel, CatalogSettings>()
                .ForMember(settings => settings.AjaxProcessAttributeChange, options => options.Ignore())
                .ForMember(settings => settings.CompareProductsNumber, options => options.Ignore())
                .ForMember(settings => settings.CountDisplayedYearsDatePicker, options => options.Ignore())
                .ForMember(settings => settings.DefaultCategoryPageSize, options => options.Ignore())
                .ForMember(settings => settings.DefaultCategoryPageSizeOptions, options => options.Ignore())
                .ForMember(settings => settings.DefaultManufacturerPageSize, options => options.Ignore())
                .ForMember(settings => settings.DefaultManufacturerPageSizeOptions, options => options.Ignore())
                .ForMember(settings => settings.DefaultProductRatingValue, options => options.Ignore())
                .ForMember(settings => settings.DisplayTierPricesWithDiscounts, options => options.Ignore())
                .ForMember(settings => settings.ExportImportProductsCountInOneFile, options => options.Ignore())
                .ForMember(settings => settings.ExportImportUseDropdownlistsForAssociatedEntities, options => options.Ignore())
                .ForMember(settings => settings.IncludeFeaturedProductsInNormalLists, options => options.Ignore())
                .ForMember(settings => settings.MaximumBackInStockSubscriptions, options => options.Ignore())
                .ForMember(settings => settings.ProductSortingEnumDisabled, options => options.Ignore())
                .ForMember(settings => settings.ProductSortingEnumDisplayOrder, options => options.Ignore())
                .ForMember(settings => settings.PublishBackProductWhenCancellingOrders, options => options.Ignore())
                .ForMember(settings => settings.UseAjaxLoadMenu, options => options.Ignore())
                .ForMember(settings => settings.UseLinksInRequiredProductWarnings, options => options.Ignore());

            CreateMap<ProductCategory, CategoryProductModel>()
                .ForMember(model => model.ProductName, options => options.Ignore());
            CreateMap<CategoryProductModel, ProductCategory>()
                .ForMember(entity => entity.CategoryId, options => options.Ignore())
                .ForMember(entity => entity.ProductId, options => options.Ignore());

            CreateMap<Category, CategoryModel>()
                .ForMember(model => model.AvailableCategories, options => options.Ignore())
                .ForMember(model => model.AvailableCategoryTemplates, options => options.Ignore())
                .ForMember(model => model.Breadcrumb, options => options.Ignore())
                .ForMember(model => model.CategoryProductSearchModel, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.Ignore());
            CreateMap<CategoryModel, Category>()
                .ForMember(entity => entity.CreatedOn, options => options.Ignore())
                .ForMember(entity => entity.Deleted, options => options.Ignore())
                .ForMember(entity => entity.UpdatedOn, options => options.Ignore());

            CreateMap<CategoryTemplate, CategoryTemplateModel>();
            CreateMap<CategoryTemplateModel, CategoryTemplate>();

            CreateMap<ProductManufacturer, ManufacturerProductModel>()
                .ForMember(model => model.ProductName, options => options.Ignore());
            CreateMap<ManufacturerProductModel, ProductManufacturer>()
                .ForMember(entity => entity.ManufacturerId, options => options.Ignore())
                .ForMember(entity => entity.ProductId, options => options.Ignore());

            CreateMap<Manufacturer, ManufacturerModel>()
                .ForMember(model => model.AvailableManufacturerTemplates, options => options.Ignore())
                .ForMember(model => model.ManufacturerProductSearchModel, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.Ignore());
            CreateMap<ManufacturerModel, Manufacturer>()
                .ForMember(entity => entity.CreatedOn, options => options.Ignore())
                .ForMember(entity => entity.Deleted, options => options.Ignore())
                .ForMember(entity => entity.UpdatedOn, options => options.Ignore());

            CreateMap<ManufacturerTemplate, ManufacturerTemplateModel>();
            CreateMap<ManufacturerTemplateModel, ManufacturerTemplate>();

            //Review type
            CreateMap<ReviewType, ReviewTypeModel>();
            CreateMap<ReviewTypeModel, ReviewType>();

            //product review
            CreateMap<ProductReview, ProductReviewModel>()
                .ForMember(model => model.UserInfo, mo => mo.Ignore())
                .ForMember(model => model.IsLoggedInAsVendor, mo => mo.Ignore())
                .ForMember(model => model.ProductReviewReviewTypeMappingSearchModel, mo => mo.Ignore())
                .ForMember(model => model.CreatedOn, mo => mo.Ignore())
                .ForMember(model => model.StoreName, mo => mo.Ignore())
                .ForMember(model => model.ProductName, mo => mo.Ignore());

            //product review type mapping
            CreateMap<ProductReviewReviewType, ProductReviewReviewTypeMappingModel>()
                .ForMember(model => model.Name, mo => mo.Ignore())
                .ForMember(model => model.Description, mo => mo.Ignore())
                .ForMember(model => model.VisibleToAllUsers, mo => mo.Ignore());

            //products
            CreateMap<Product, ProductModel>()
                .ForMember(model => model.AddPictureModel, options => options.Ignore())
                .ForMember(model => model.AssociatedProductSearchModel, options => options.Ignore())
                .ForMember(model => model.AssociatedToProductId, options => options.Ignore())
                .ForMember(model => model.AssociatedToProductName, options => options.Ignore())
                .ForMember(model => model.AvailableBasepriceBaseUnits, options => options.Ignore())
                .ForMember(model => model.AvailableBasepriceUnits, options => options.Ignore())
                .ForMember(model => model.AvailableCategories, options => options.Ignore())
                .ForMember(model => model.AvailableDeliveryDates, options => options.Ignore())
                .ForMember(model => model.AvailableManufacturers, options => options.Ignore())
                .ForMember(model => model.AvailableProductAvailabilityRanges, options => options.Ignore())
                .ForMember(model => model.AvailableProductTemplates, options => options.Ignore())
                .ForMember(model => model.AvailableTaxCategories, options => options.Ignore())
                .ForMember(model => model.AvailableVendors, options => options.Ignore())
                .ForMember(model => model.AvailableWarehouses, options => options.Ignore())
                .ForMember(model => model.BaseDimensionIn, options => options.Ignore())
                .ForMember(model => model.BaseWeightIn, options => options.Ignore())
                .ForMember(model => model.CopyProductModel, options => options.Ignore())
                .ForMember(model => model.CrossSellProductSearchModel, options => options.Ignore())
                .ForMember(model => model.HasAvailableSpecificationAttributes, options => options.Ignore())
                .ForMember(model => model.IsLoggedInAsVendor, options => options.Ignore())
                .ForMember(model => model.LastStockQuantity, options => options.Ignore())
                .ForMember(model => model.PictureThumbnailUrl, options => options.Ignore())
                .ForMember(model => model.PrimaryStoreCurrencyCode, options => options.Ignore())
                .ForMember(model => model.ProductAttributeCombinationSearchModel, options => options.Ignore())
                .ForMember(model => model.ProductAttributeMappingSearchModel, options => options.Ignore())
                .ForMember(model => model.ProductAttributesExist, options => options.Ignore())
                .ForMember(model => model.CanCreateCombinations, options => options.Ignore())
                .ForMember(model => model.ProductEditorSettingsModel, options => options.Ignore())
                .ForMember(model => model.ProductOrderSearchModel, options => options.Ignore())
                .ForMember(model => model.ProductPictureModels, options => options.Ignore())
                .ForMember(model => model.ProductPictureSearchModel, options => options.Ignore())
                .ForMember(model => model.ProductSpecificationAttributeSearchModel, options => options.Ignore())
                .ForMember(model => model.ProductsTypesSupportedByProductTemplates, options => options.Ignore())
                .ForMember(model => model.ProductTags, options => options.Ignore())
                .ForMember(model => model.ProductTypeName, options => options.Ignore())
                .ForMember(model => model.ProductWarehouseInventoryModels, options => options.Ignore())
                .ForMember(model => model.RelatedProductSearchModel, options => options.Ignore())
                .ForMember(model => model.SelectedCategoryIds, options => options.Ignore())
                .ForMember(model => model.SelectedManufacturerIds, options => options.Ignore())
                .ForMember(model => model.SeName, options => options.Ignore())
                .ForMember(model => model.StockQuantityHistory, options => options.Ignore())
                .ForMember(model => model.StockQuantityHistorySearchModel, options => options.Ignore())
                .ForMember(model => model.StockQuantityStr, options => options.Ignore())
                .ForMember(model => model.TierPriceSearchModel, options => options.Ignore())
                .ForMember(model => model.InitialProductTags, options => options.Ignore());
            CreateMap<ProductModel, Product>()
                .ForMember(entity => entity.ApprovedRatingSum, options => options.Ignore())
                .ForMember(entity => entity.ApprovedTotalReviews, options => options.Ignore())
                .ForMember(entity => entity.BackorderMode, options => options.Ignore())
                .ForMember(entity => entity.CreatedOn, options => options.Ignore())
                .ForMember(entity => entity.Deleted, options => options.Ignore())
                .ForMember(entity => entity.DownloadActivationType, options => options.Ignore())
                .ForMember(entity => entity.GiftCardType, options => options.Ignore())
                .ForMember(entity => entity.HasDiscountsApplied, options => options.Ignore())
                .ForMember(entity => entity.HasTierPrices, options => options.Ignore())
                .ForMember(entity => entity.LowStockActivity, options => options.Ignore())
                .ForMember(entity => entity.ManageInventoryMethod, options => options.Ignore())
                .ForMember(entity => entity.NotApprovedRatingSum, options => options.Ignore())
                .ForMember(entity => entity.NotApprovedTotalReviews, options => options.Ignore())
                .ForMember(entity => entity.ParentGroupedProductId, options => options.Ignore())
                .ForMember(entity => entity.ProductType, options => options.Ignore())
                .ForMember(entity => entity.RecurringCyclePeriod, options => options.Ignore())
                .ForMember(entity => entity.RentalPricePeriod, options => options.Ignore())
                .ForMember(entity => entity.UpdatedOn, options => options.Ignore());

            CreateMap<Product, DiscountProductModel>()
                .ForMember(model => model.ProductId, options => options.Ignore())
                .ForMember(model => model.ProductName, options => options.Ignore());

            CreateMap<Product, AssociatedProductModel>()
                .ForMember(model => model.ProductName, options => options.Ignore());

            CreateMap<ProductAttributeCombination, ProductAttributeCombinationModel>()
               .ForMember(model => model.AttributesXml, options => options.Ignore())
               .ForMember(model => model.ProductAttributes, options => options.Ignore())
               .ForMember(model => model.ProductPictureModels, options => options.Ignore())
               .ForMember(model => model.PictureThumbnailUrl, options => options.Ignore())
               .ForMember(model => model.Warnings, options => options.Ignore());
            CreateMap<ProductAttributeCombinationModel, ProductAttributeCombination>()
               .ForMember(entity => entity.AttributesXml, options => options.Ignore());

            CreateMap<ProductAttribute, ProductAttributeModel>()
                .ForMember(model => model.PredefinedProductAttributeValueSearchModel, options => options.Ignore())
                .ForMember(model => model.ProductAttributeProductSearchModel, options => options.Ignore());
            CreateMap<ProductAttributeModel, ProductAttribute>();

            CreateMap<Product, ProductAttributeProductModel>()
                .ForMember(model => model.ProductName, options => options.Ignore());

            CreateMap<PredefinedProductAttributeValue, PredefinedProductAttributeValueModel>()
                .ForMember(model => model.WeightAdjustmentStr, options => options.Ignore())
                .ForMember(model => model.PriceAdjustmentStr, options => options.Ignore());
            CreateMap<PredefinedProductAttributeValueModel, PredefinedProductAttributeValue>();

            CreateMap<ProductProductAttribute, ProductAttributeMappingModel>()
                .ForMember(model => model.ValidationRulesString, options => options.Ignore())
                .ForMember(model => model.AttributeControlType, options => options.Ignore())
                .ForMember(model => model.ConditionString, options => options.Ignore())
                .ForMember(model => model.ProductAttribute, options => options.Ignore())
                .ForMember(model => model.AvailableProductAttributes, options => options.Ignore())
                .ForMember(model => model.ConditionAllowed, options => options.Ignore())
                .ForMember(model => model.ConditionModel, options => options.Ignore())
                .ForMember(model => model.ProductAttributeValueSearchModel, options => options.Ignore());
            CreateMap<ProductAttributeMappingModel, ProductProductAttribute>()
                .ForMember(entity => entity.ConditionAttributeXml, options => options.Ignore())
                .ForMember(entity => entity.AttributeControlType, options => options.Ignore());

            CreateMap<ProductAttributeValue, ProductAttributeValueModel>()
                .ForMember(model => model.AttributeValueTypeName, options => options.Ignore())
                .ForMember(model => model.Name, options => options.Ignore())
                .ForMember(model => model.PriceAdjustmentStr, options => options.Ignore())
                .ForMember(model => model.AssociatedProductName, options => options.Ignore())
                .ForMember(model => model.PictureThumbnailUrl, options => options.Ignore())
                .ForMember(model => model.WeightAdjustmentStr, options => options.Ignore())
                .ForMember(model => model.DisplayColorSquaresRgb, options => options.Ignore())
                .ForMember(model => model.DisplayImageSquaresPicture, options => options.Ignore())
                .ForMember(model => model.ProductPictureModels, options => options.Ignore());
            CreateMap<ProductAttributeValueModel, ProductAttributeValue>()
               .ForMember(entity => entity.AttributeValueType, options => options.Ignore())
               .ForMember(entity => entity.Quantity, options => options.Ignore());

            CreateMap<ProductEditorSettings, ProductEditorSettingsModel>();
            CreateMap<ProductEditorSettingsModel, ProductEditorSettings>();

            CreateMap<ProductPicture, ProductPictureModel>()
                .ForMember(model => model.OverrideAltAttribute, options => options.Ignore())
                .ForMember(model => model.OverrideTitleAttribute, options => options.Ignore())
                .ForMember(model => model.PictureUrl, options => options.Ignore());

            CreateMap<Product, SpecificationAttributeProductModel>()
                .ForMember(model => model.SpecificationAttributeId, options => options.Ignore())
                .ForMember(model => model.ProductId, options => options.Ignore())
                .ForMember(model => model.ProductName, options => options.Ignore());

            CreateMap<ProductSpecificationAttribute, ProductSpecificationAttributeModel>()
                .ForMember(model => model.AttributeTypeName, options => options.Ignore())
                .ForMember(model => model.ValueRaw, options => options.Ignore())
                .ForMember(model => model.AttributeId, options => options.Ignore())
                .ForMember(model => model.AttributeName, options => options.Ignore())
                .ForMember(model => model.SpecificationAttributeOptionId, options => options.Ignore());

            CreateMap<ProductSpecificationAttribute, AddSpecificationAttributeModel>()
                .ForMember(entity => entity.SpecificationId, options => options.Ignore())
                .ForMember(entity => entity.AttributeTypeName, options => options.Ignore())
                .ForMember(entity => entity.AttributeId, options => options.Ignore())
                .ForMember(entity => entity.AttributeName, options => options.Ignore())
                .ForMember(entity => entity.ValueRaw, options => options.Ignore())
                .ForMember(entity => entity.Value, options => options.Ignore())
                .ForMember(entity => entity.AvailableOptions, options => options.Ignore())
                .ForMember(entity => entity.AvailableAttributes, options => options.Ignore());

            CreateMap<AddSpecificationAttributeModel, ProductSpecificationAttribute>()
                .ForMember(model => model.CustomValue, options => options.Ignore())
                .ForMember(model => model.AttributeType, options => options.Ignore());

            CreateMap<ProductTag, ProductTagModel>()
               .ForMember(model => model.ProductCount, options => options.Ignore());

            CreateMap<ProductTemplate, ProductTemplateModel>();
            CreateMap<ProductTemplateModel, ProductTemplate>();

            CreateMap<RelatedProduct, RelatedProductModel>()
               .ForMember(model => model.Product2Name, options => options.Ignore());

            CreateMap<SpecificationAttribute, SpecificationAttributeModel>()
                .ForMember(model => model.SpecificationAttributeOptionSearchModel, options => options.Ignore())
                .ForMember(model => model.SpecificationAttributeProductSearchModel, options => options.Ignore());
            CreateMap<SpecificationAttributeModel, SpecificationAttribute>();

            CreateMap<SpecificationAttributeOption, SpecificationAttributeOptionModel>()
                .ForMember(model => model.EnableColorSquaresRgb, options => options.Ignore())
                .ForMember(model => model.NumberOfAssociatedProducts, options => options.Ignore());
            CreateMap<SpecificationAttributeOptionModel, SpecificationAttributeOption>();

            CreateMap<StockQuantityHistory, StockQuantityHistoryModel>()
                .ForMember(model => model.WarehouseName, options => options.Ignore())
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.AttributeCombination, options => options.Ignore());

            CreateMap<TierPrice, TierPriceModel>()
                .ForMember(model => model.Store, options => options.Ignore())
                .ForMember(model => model.AvailableUserRoles, options => options.Ignore())
                .ForMember(model => model.AvailableStores, options => options.Ignore())
                .ForMember(model => model.UserRole, options => options.Ignore());
            CreateMap<TierPriceModel, TierPrice>()
                .ForMember(entity => entity.UserRoleId, options => options.Ignore())
                .ForMember(entity => entity.ProductId, options => options.Ignore());
        }
        #endregion

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
}
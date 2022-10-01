using Microsoft.EntityFrameworkCore;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Affiliates;
using WCore.Core.Domain.Catalog;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Congresses;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.Discounts;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Logging;
using WCore.Core.Domain.Medias;
using WCore.Core.Domain.Messages;
using WCore.Core.Domain.Newses;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Popup;
using WCore.Core.Domain.Roles;
using WCore.Core.Domain.Seo;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Shipping;
using WCore.Core.Domain.Stores;
using WCore.Core.Domain.Tasks;
using WCore.Core.Domain.Tax;
using WCore.Core.Domain.Teams;
using WCore.Core.Domain.Templates;
using WCore.Core.Domain.Users;
using WCore.Core.Domain.Vendors;

namespace WCore.Core
{
    public class WCoreContext : DbContext
    {
        public WCoreContext(DbContextOptions<WCoreContext> options) : base(options)
        {
        }

        #region User
        public DbSet<User> Users { get; set; }
        public DbSet<UserRegistrationForm> UserRegistrationForms { get; set; }
        public DbSet<UserAgencyAuthorization> UserAgencyAuthorizations { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserAttribute> UserAttributes { get; set; }
        public DbSet<UserAttributeValue> UserAttributeValues { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<ExternalAuthenticationRecord> ExternalAuthenticationRecords { get; set; }
        public DbSet<RewardPointsHistory> RewardPointsHistories { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ActivityLogType> ActivityLogTypes { get; set; }
        #endregion

        #region Role
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<TempRole> TempRoles { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<TempRoleGroup> TempRoleGroups { get; set; }
        #endregion

        #region Academy
        public DbSet<Academy> Academies { get; set; }
        public DbSet<AcademyCategory> AcademyCategories { get; set; }
        public DbSet<AcademyFile> AcademyFiles { get; set; }
        public DbSet<AcademyImage> AcademyImages { get; set; }
        public DbSet<AcademyVideo> AcademyVideos { get; set; }

        #endregion

        #region News
        public DbSet<News> Newses { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsImage> NewsImages { get; set; }
        #endregion

        #region Team
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamCategory> TeamCategories { get; set; }
        #endregion

        #region Congress
        public DbSet<Congress> Congresses { get; set; }
        public DbSet<CongressPaperType> CongressPaperTypes { get; set; }
        public DbSet<CongressPaper> CongressPapers { get; set; }
        public DbSet<CongressPresentationType> CongressPresentationTypes { get; set; }
        public DbSet<CongressPresentation> CongressPresentations { get; set; }
        public DbSet<CongressImage> CongressImages { get; set; }

        #endregion

        #region Common
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressAttribute> AddressAttributes { get; set; }
        public DbSet<AddressAttributeValue> AddressAttributeValues { get; set; }
        public DbSet<SearchTerm> SearchTerms { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }

        #endregion

        #region Catalog
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTemplate> CategoryTemplates { get; set; }
        public DbSet<CrossSellProduct> CrossSellProducts { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ManufacturerTemplate> ManufacturerTemplates { get; set; }
        public DbSet<PredefinedProductAttributeValue> PredefinedProductAttributeValues { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeCombination> ProductAttributeCombinations { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductProductAttribute> ProductProductAttributes { get; set; }
        public DbSet<ProductReviewHelpfulness> ProductReviewHelpfulnesses { get; set; }
        public DbSet<ProductReviewReviewType> ProductReviewReviewTypes { get; set; }
        public DbSet<ProductSpecificationAttribute> ProductSpecificationAttributes { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductProductTag> ProductProductTags { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductManufacturer> ProductManufacturers { get; set; }
        public DbSet<ProductTemplate> ProductTemplates { get; set; }
        public DbSet<ProductWarehouseInventory> ProductWarehouseInventories { get; set; }
        public DbSet<RelatedProduct> RelatedProducts { get; set; }
        public DbSet<ReviewType> ReviewTypes { get; set; }
        public DbSet<SpecificationAttribute> SpecificationAttributes { get; set; }
        public DbSet<SpecificationAttributeOption> SpecificationAttributeOptions { get; set; }
        public DbSet<StockQuantityHistory> StockQuantityHistories { get; set; }
        public DbSet<TierPrice> TierPrices { get; set; }

        #endregion

        #region Discount
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountAppliedToCategory> DiscountAppliedToCategories { get; set; }
        public DbSet<DiscountAppliedToManufacturer> DiscountAppliedToManufacturers { get; set; }
        public DbSet<DiscountAppliedToProduct> DiscountAppliedToProducts { get; set; }
        public DbSet<DiscountRequirement> DiscountRequirements { get; set; }
        public DbSet<DiscountUsageHistory> DiscountUsageHistories { get; set; }

        #endregion

        #region Order
        public DbSet<CheckoutAttribute> CheckoutAttributes { get; set; }
        public DbSet<CheckoutAttributeValue> CheckoutAttributeValues { get; set; }
        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<GiftCardUsageHistory> GiftCardUsageHistories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderNote> OrderNotes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<RecurringPayment> RecurringPayments { get; set; }
        public DbSet<RecurringPaymentHistory> RecurringPaymentHistories { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<ReturnRequestAction> ReturnRequestActions { get; set; }
        public DbSet<ReturnRequestReason> ReturnRequestReasons { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        #endregion

        #region Shipping
        public DbSet<DeliveryDate> DeliveryDates { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentItem> ShipmentItems { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        #endregion

        #region Tax
        public DbSet<TaxCategory> TaxCategories { get; set; }

        #endregion

        #region Directory
        public DbSet<MeasureDimension> MeasureDimensions { get; set; }
        public DbSet<MeasureWeight> MeasureWeights { get; set; }

        #endregion

        #region Vendor
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorAttribute> VendorAttributes { get; set; }
        public DbSet<VendorAttributeValue> VendorAttributeValues { get; set; }
        public DbSet<VendorNote> VendorNotes { get; set; }
        #endregion

        #region Store
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreMapping> StoreMappings { get; set; }
        #endregion

        #region Media
        public DbSet<Download> Downloads { get; set; }

        #endregion

        #region Messages
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<NewsLetterSubscription> NewsLetterSubscriptions { get; set; }
        public DbSet<MessageTemplate> MessageTemplates { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<QueuedEmail> QueuedEmails { get; set; }

        #endregion

        #region Content

        public DbSet<Popup> Popups { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<DynamicForm> DynamicForms { get; set; }
        public DbSet<DynamicFormElement> DynamicFormElements { get; set; }
        public DbSet<DynamicFormRecord> DynamicFormRecords { get; set; }

        #endregion

        #region Language
        public DbSet<LocalizedProperty> LocalizedProperties { get; set; }
        public DbSet<LocaleStringResource> LocaleStringResources { get; set; }
        public DbSet<Language> Languages { get; set; }

        #endregion

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UrlRecord> UrlRecords { get; set; }
        public DbSet<GenericAttribute> GenericAttributes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<ScheduleTask> ScheduleTasks { get; set; }
        public DbSet<Template> Templates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

    }
}

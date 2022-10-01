using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Configuration;
using WCore.Core.Infrastructure;
using WCore.Core.Infrastructure.DependencyManagement;
using WCore.Framework.Factories;
using WCore.Framework.Mvc.Routing;
using WCore.Framework.Themes;
using WCore.Framework.UI;
using WCore.Services;
using WCore.Services.Academies;
using WCore.Services.Affiliates;
using WCore.Services.Authentication;
using WCore.Services.Caching;
using WCore.Services.Catalog;
using WCore.Services.Common;
using WCore.Services.Congresses;
using WCore.Services.Directory;
using WCore.Services.Discounts;
using WCore.Services.DynamicForms;
using WCore.Services.Events;
using WCore.Services.Galleries;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Logging;
using WCore.Services.Medias;
using WCore.Services.Menus;
using WCore.Services.Messages;
using WCore.Services.Newses;
using WCore.Services.Orders;
using WCore.Services.Pages;
using WCore.Services.Payments;
using WCore.Services.Plugins;
using WCore.Services.Plugins.Marketplace;
using WCore.Services.Popups;
using WCore.Services.Roles;
using WCore.Services.RoxyFileman;
using WCore.Services.Security;
using WCore.Services.Seo;
using WCore.Services.Settings;
using WCore.Services.Shipping;
using WCore.Services.Shipping.Date;
using WCore.Services.Shipping.Pickup;
using WCore.Services.Stores;
using WCore.Services.Tasks;
using WCore.Services.Tax;
using WCore.Services.Teams;
using WCore.Services.Templates;
using WCore.Services.Users;
using WCore.Services.Vendors;

namespace WCore.Framework.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, AppSettings appSettings)
        {
            //file provider
            builder.RegisterType<WCoreFileProvider>().As<IWCoreFileProvider>().InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //user agent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();


            ////repositories
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //plugins
            builder.RegisterType<PluginService>().As<IPluginService>().InstancePerLifetimeScope();
            builder.RegisterType<OfficialFeedManager>().AsSelf().InstancePerLifetimeScope();


            builder.RegisterType<MemoryCacheManager>().As<ILocker>().As<IStaticCacheManager>().SingleInstance();

            //work context
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            builder.RegisterType<StoreContext>().As<IStoreContext>().InstancePerLifetimeScope();
            builder.RegisterType<StoreMappingService>().As<IStoreMappingService>().InstancePerLifetimeScope();
            builder.RegisterType<StoreService>().As<IStoreService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizedModelFactory>().As<ILocalizedModelFactory>().InstancePerLifetimeScope();

            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizedEntityService>().As<ILocalizedEntityService>().InstancePerLifetimeScope();
            builder.RegisterType<UrlRecordService>().As<IUrlRecordService>().InstancePerLifetimeScope();

            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleGroupService>().As<IRoleGroupService>().InstancePerLifetimeScope();

            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();

            builder.RegisterType<TemplateService>().As<ITemplateService>().InstancePerLifetimeScope();

            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerLifetimeScope();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().SingleInstance();

            builder.RegisterType<CookieAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();

            // user
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRegistrationFormService>().As<IUserRegistrationFormService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRegistrationService>().As<IUserRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserAttributeService>().As<IUserAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<UserAttributeParser>().As<IUserAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<UserAgencyService>().As<IUserAgencyService>().InstancePerLifetimeScope();
            builder.RegisterType<UserAgencyAuthorizationService>().As<IUserAgencyAuthorizationService>().InstancePerLifetimeScope();
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeService>().As<IAddressAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeParser>().As<IAddressAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<UserActivityService>().As<IUserActivityService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsLetterSubscriptionService>().As<INewsLetterSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<RewardPointService>().As<IRewardPointService>().InstancePerLifetimeScope();

            //catalog
            builder.RegisterType<TaxService>().As<ITaxService>().InstancePerLifetimeScope();

            //order
            builder.RegisterType<ShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();

            // message
            builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>().InstancePerLifetimeScope();

            // popup
            builder.RegisterType<PopupService>().As<IPopupService>().InstancePerLifetimeScope();


            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerLifetimeScope();
            builder.RegisterType<CityService>().As<ICityService>().InstancePerLifetimeScope();
            builder.RegisterType<StateProvinceService>().As<IStateProvinceService>().InstancePerLifetimeScope();

            builder.RegisterType<SlugRouteTransformer>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();

            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();

            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();

            builder.RegisterType<TemplateService>().As<ITemplateService>().InstancePerLifetimeScope();

            builder.RegisterType<PageService>().As<IPageService>().InstancePerLifetimeScope();
            builder.RegisterType<GalleryService>().As<IGalleryService>().InstancePerLifetimeScope();
            builder.RegisterType<GalleryImageService>().As<IGalleryImageService>().InstancePerLifetimeScope();
            builder.RegisterType<DynamicFormService>().As<IDynamicFormService>().InstancePerLifetimeScope();
            builder.RegisterType<DynamicFormElementService>().As<IDynamicFormElementService>().InstancePerLifetimeScope();
            builder.RegisterType<DynamicFormRecordService>().As<IDynamicFormRecordService>().InstancePerLifetimeScope();

            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            builder.RegisterType<FileRoxyFilemanService>().As<IRoxyFilemanService>().InstancePerLifetimeScope();

            builder.RegisterType<CacheKeyService>().As<ICacheKeyService>().InstancePerLifetimeScope();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();

            //vendor
            builder.RegisterType<VendorAttributeFormatter>().As<IVendorAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeParser>().As<IVendorAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeService>().As<IVendorAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<VendorService>().As<IVendorService>().InstancePerLifetimeScope();

            //congress
            builder.RegisterType<CongressService>().As<ICongressService>().InstancePerLifetimeScope();
            builder.RegisterType<CongressImageService>().As<ICongressImageService>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPaperTypeService>().As<ICongressPaperTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPaperService>().As<ICongressPaperService>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPresentationTypeService>().As<ICongressPresentationTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPresentationService>().As<ICongressPresentationService>().InstancePerLifetimeScope();

            //news
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsCategoryService>().As<INewsCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsImageService>().As<INewsImageService>().InstancePerLifetimeScope();

            //team
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
            builder.RegisterType<TeamCategoryService>().As<ITeamCategoryService>().InstancePerLifetimeScope();

            //academy
            builder.RegisterType<AcademyService>().As<IAcademyService>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyCategoryService>().As<IAcademyCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyImageService>().As<IAcademyImageService>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyFileService>().As<IAcademyFileService>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyVideoService>().As<IAcademyVideoService>().InstancePerLifetimeScope();




            //services
            builder.RegisterType<BackInStockSubscriptionService>().As<IBackInStockSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<CompareProductsService>().As<ICompareProductsService>().InstancePerLifetimeScope();
            builder.RegisterType<RecentlyViewedProductsService>().As<IRecentlyViewedProductsService>().InstancePerLifetimeScope();
            builder.RegisterType<ManufacturerService>().As<IManufacturerService>().InstancePerLifetimeScope();
            builder.RegisterType<PriceFormatter>().As<IPriceFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<ProductAttributeFormatter>().As<IProductAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<ProductAttributeParser>().As<IProductAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<ProductAttributeService>().As<IProductAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<CopyProductService>().As<ICopyProductService>().InstancePerLifetimeScope();
            builder.RegisterType<SpecificationAttributeService>().As<ISpecificationAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductTemplateService>().As<IProductTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryTemplateService>().As<ICategoryTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<ManufacturerTemplateService>().As<IManufacturerTemplateService>().InstancePerLifetimeScope();
            //builder.RegisterType<TopicTemplateService>().As<ITopicTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductTagService>().As<IProductTagService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeFormatter>().As<IAddressAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeParser>().As<IAddressAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeService>().As<IAddressAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<AffiliateService>().As<IAffiliateService>().InstancePerLifetimeScope();
            builder.RegisterType<VendorService>().As<IVendorService>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeFormatter>().As<IVendorAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeParser>().As<IVendorAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeService>().As<IVendorAttributeService>().InstancePerLifetimeScope();
            //builder.RegisterType<SearchTermService>().As<ISearchTermService>().InstancePerLifetimeScope();
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerLifetimeScope();
            //builder.RegisterType<FulltextService>().As<IFulltextService>().InstancePerLifetimeScope();
            //builder.RegisterType<MaintenanceService>().As<IMaintenanceService>().InstancePerLifetimeScope();
            builder.RegisterType<UserAttributeFormatter>().As<IUserAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<UserAttributeParser>().As<IUserAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<UserAttributeService>().As<IUserAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRegistrationService>().As<IUserRegistrationService>().InstancePerLifetimeScope();
            //builder.RegisterType<UserReportService>().As<IUserReportService>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            builder.RegisterType<PriceCalculationService>().As<IPriceCalculationService>().InstancePerLifetimeScope();
            builder.RegisterType<GeoLookupService>().As<IGeoLookupService>().InstancePerLifetimeScope();
            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<MeasureService>().As<IMeasureService>().InstancePerLifetimeScope();
            builder.RegisterType<StateProvinceService>().As<IStateProvinceService>().InstancePerLifetimeScope();
            builder.RegisterType<StoreService>().As<IStoreService>().InstancePerLifetimeScope();
            builder.RegisterType<StoreMappingService>().As<IStoreMappingService>().InstancePerLifetimeScope();
            builder.RegisterType<DiscountService>().As<IDiscountService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizedEntityService>().As<ILocalizedEntityService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadService>().As<IDownloadService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<QueuedEmailService>().As<IQueuedEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsLetterSubscriptionService>().As<INewsLetterSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();
            builder.RegisterType<CampaignService>().As<ICampaignService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailAccountService>().As<IEmailAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkflowMessageService>().As<IWorkflowMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTokenProvider>().As<IMessageTokenProvider>().InstancePerLifetimeScope();
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerLifetimeScope();
            builder.RegisterType<SmtpBuilder>().As<ISmtpBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterType<CheckoutAttributeFormatter>().As<ICheckoutAttributeFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<CheckoutAttributeParser>().As<ICheckoutAttributeParser>().InstancePerLifetimeScope();
            builder.RegisterType<CheckoutAttributeService>().As<ICheckoutAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<GiftCardService>().As<IGiftCardService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderReportService>().As<IOrderReportService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderProcessingService>().As<IOrderProcessingService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderTotalCalculationService>().As<IOrderTotalCalculationService>().InstancePerLifetimeScope();
            builder.RegisterType<ReturnRequestService>().As<IReturnRequestService>().InstancePerLifetimeScope();
            builder.RegisterType<RewardPointService>().As<IRewardPointService>().InstancePerLifetimeScope();
            builder.RegisterType<ShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomNumberFormatter>().As<ICustomNumberFormatter>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentService>().As<IPaymentService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<CookieAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<UrlRecordService>().As<IUrlRecordService>().InstancePerLifetimeScope();
            builder.RegisterType<ShipmentService>().As<IShipmentService>().InstancePerLifetimeScope();
            builder.RegisterType<ShippingService>().As<IShippingService>().InstancePerLifetimeScope();
            builder.RegisterType<DateRangeService>().As<IDateRangeService>().InstancePerLifetimeScope();
            builder.RegisterType<TaxCategoryService>().As<ITaxCategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<TaxService>().As<ITaxService>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultLogger>().As<ILogger>().InstancePerLifetimeScope();
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();
            //builder.RegisterType<SitemapGenerator>().As<ISitemapGenerator>().InstancePerLifetimeScope();
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();
            //builder.RegisterType<ExportManager>().As<IExportManager>().InstancePerLifetimeScope();
            //builder.RegisterType<ImportManager>().As<IImportManager>().InstancePerLifetimeScope();
            builder.RegisterType<PdfService>().As<IPdfService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadService>().As<IUploadService>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerLifetimeScope();
            //builder.RegisterType<ExternalAuthenticationService>().As<IExternalAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();
            builder.RegisterType<CacheKeyService>().As<ICacheKeyService>().InstancePerLifetimeScope();
            //slug route transformer
            builder.RegisterType<SlugRouteTransformer>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ReviewTypeService>().As<IReviewTypeService>().SingleInstance();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SettingService>().As<ISettingService>().InstancePerLifetimeScope();



            //plugin managers
            builder.RegisterGeneric(typeof(PluginManager<>)).As(typeof(IPluginManager<>)).InstancePerLifetimeScope();
            //builder.RegisterType<AuthenticationPluginManager>().As<IAuthenticationPluginManager>().InstancePerLifetimeScope();
            //builder.RegisterType<WidgetPluginManager>().As<IWidgetPluginManager>().InstancePerLifetimeScope();
            //builder.RegisterType<ExchangeRatePluginManager>().As<IExchangeRatePluginManager>().InstancePerLifetimeScope();
            builder.RegisterType<DiscountPluginManager>().As<IDiscountPluginManager>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentPluginManager>().As<IPaymentPluginManager>().InstancePerLifetimeScope();
            builder.RegisterType<PickupPluginManager>().As<IPickupPluginManager>().InstancePerLifetimeScope();
            builder.RegisterType<ShippingPluginManager>().As<IShippingPluginManager>().InstancePerLifetimeScope();
            builder.RegisterType<TaxPluginManager>().As<ITaxPluginManager>().InstancePerLifetimeScope();

            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>().InstancePerLifetimeScope();

            //register all settings
            builder.RegisterSource(new SettingsSource());


            //installation service
            //if (!DataSettingsManager.DatabaseIsInstalled)
            //{
            //    if (config.UseFastInstallationService)
            //        builder.RegisterType<SqlFileInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            //    else
            //        builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            //}

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }


    /// <summary>
    /// Setting source
    /// </summary>
    public class SettingsSource : IRegistrationSource
    {
        private static readonly MethodInfo _buildMethod = typeof(SettingsSource).GetMethod("BuildRegistration", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Registrations for
        /// </summary>
        /// <param name="service">Service</param>
        /// <param name="registrations">Registrations</param>
        /// <returns>Registrations</returns>
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service,
            Func<Service, IEnumerable<ServiceRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = _buildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        private static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    //uncomment the code below if you want load settings per store only when you have two stores installed.
                    //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
                    //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

                    //although it's better to connect to your database and execute the following SQL:
                    //DELETE FROM [Setting] WHERE [StoreId] > 0
                    try
                    {
                        var resolveISetting = c.Resolve<ISettingService>();
                        var settings = resolveISetting.LoadSetting<TSettings>();
                        return settings;
                    }
                    catch
                    {
                        //if (DataSettingsManager.DatabaseIsInstalled)
                        //    throw;
                    }

                    return default;
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        /// <summary>
        /// Is adapter for individual components
        /// </summary>
        public bool IsAdapterForIndividualComponents => false;
    }
}

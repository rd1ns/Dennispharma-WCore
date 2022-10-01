using Autofac;
using WCore.Core.Configuration;
using WCore.Core.Infrastructure;
using WCore.Core.Infrastructure.DependencyManagement;
using WCore.Framework.Factories;
using WCore.Framework.Models;
using WCore.Web.Areas.Admin.Factories;
using WCore.Web.Factories;

namespace WCore.Web.Infrastructure
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
            builder.RegisterType<LocalizedModelFactory>().As<ILocalizedModelFactory>().InstancePerLifetimeScope();

            //common factories
            builder.RegisterType<DiscountSupportedModelFactory>().As<IDiscountSupportedModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizedModelFactory>().As<ILocalizedModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<StoreMappingSupportedModelFactory>().As<IStoreMappingSupportedModelFactory>().InstancePerLifetimeScope();

            //admin factories
            builder.RegisterType<BaseAdminModelFactory>().As<IBaseAdminModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryModelFactory>().As<ICategoryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CommonModelFactory>().As<ICommonModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyModelFactory>().As<ICurrencyModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UserAttributeModelFactory>().As<IUserAttributeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UserModelFactory>().As<IUserModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<UserRoleModelFactory>().As<IUserRoleModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<DiscountModelFactory>().As<IDiscountModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<EmailAccountModelFactory>().As<IEmailAccountModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ExternalAuthenticationMethodModelFactory>().As<IExternalAuthenticationMethodModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ForumModelFactory>().As<IForumModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<GiftCardModelFactory>().As<IGiftCardModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<HomeModelFactory>().As<IHomeModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<LanguageModelFactory>().As<ILanguageModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<LogModelFactory>().As<ILogModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ManufacturerModelFactory>().As<IManufacturerModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<MeasureModelFactory>().As<IMeasureModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<MessageTemplateModelFactory>().As<IMessageTemplateModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<NewsletterSubscriptionModelFactory>().As<INewsletterSubscriptionModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<NewsModelFactory>().As<INewsModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<OrderModelFactory>().As<IOrderModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<PaymentModelFactory>().As<IPaymentModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<PluginModelFactory>().As<IPluginModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<PollModelFactory>().As<IPollModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ProductModelFactory>().As<IProductModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ProductAttributeModelFactory>().As<IProductAttributeModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ProductReviewModelFactory>().As<IProductReviewModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ReportModelFactory>().As<IReportModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<QueuedEmailModelFactory>().As<IQueuedEmailModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<RecurringPaymentModelFactory>().As<IRecurringPaymentModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ReturnRequestModelFactory>().As<IReturnRequestModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ReviewTypeModelFactory>().As<IReviewTypeModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ScheduleTaskModelFactory>().As<IScheduleTaskModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<SecurityModelFactory>().As<ISecurityModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SettingModelFactory>().As<ISettingModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ShippingModelFactory>().As<IShippingModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<ShoppingCartModelFactory>().As<IShoppingCartModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<SpecificationAttributeModelFactory>().As<ISpecificationAttributeModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<StoreModelFactory>().As<IStoreModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<TaxModelFactory>().As<ITaxModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<TemplateModelFactory>().As<ITemplateModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<TopicModelFactory>().As<ITopicModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeModelFactory>().As<IVendorAttributeModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<VendorModelFactory>().As<IVendorModelFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<WidgetModelFactory>().As<IWidgetModelFactory>().InstancePerLifetimeScope();

            builder.RegisterType<UserModelFactory>().As<IUserModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CommonModelFactory>().As<ICommonModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyModelFactory>().As<ICurrencyModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<GalleryModelFactory>().As<IGalleryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<GalleryImageModelFactory>().As<IGalleryImageModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<DynamicFormModelFactory>().As<IDynamicFormModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<DynamicFormElementModelFactory>().As<IDynamicFormElementModelFactory>().InstancePerLifetimeScope();

            //News
            builder.RegisterType<NewsModelFactory>().As<INewsModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<NewsCategoryModelFactory>().As<INewsCategoryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<NewsImageModelFactory>().As<INewsImageModelFactory>().InstancePerLifetimeScope();

            //Team
            builder.RegisterType<TeamModelFactory>().As<ITeamModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TeamCategoryModelFactory>().As<ITeamCategoryModelFactory>().InstancePerLifetimeScope();

            //Academy
            builder.RegisterType<AcademyModelFactory>().As<IAcademyModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyCategoryModelFactory>().As<IAcademyCategoryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyImageModelFactory>().As<IAcademyImageModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyFileModelFactory>().As<IAcademyFileModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AcademyVideoModelFactory>().As<IAcademyVideoModelFactory>().InstancePerLifetimeScope();

            //Congress
            builder.RegisterType<CongressModelFactory>().As<ICongressModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CongressImageModelFactory>().As<ICongressImageModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPaperModelFactory>().As<ICongressPaperModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPaperTypeModelFactory>().As<ICongressPaperTypeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPresentationModelFactory>().As<ICongressPresentationModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CongressPresentationTypeModelFactory>().As<ICongressPresentationTypeModelFactory>().InstancePerLifetimeScope();


            builder.RegisterType<PageModelFactory>().As<IPageModelFactory>().InstancePerLifetimeScope();
        }
        public int Order => 2;
    }
}

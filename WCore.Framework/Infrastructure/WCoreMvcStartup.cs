using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WCore.Core.Infrastructure;
using WCore.Framework.CookieManager;
using WCore.Framework.Extensions;
using WCore.Framework.Infrastructure.Extensions;

namespace WCore.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring MVC on application startup
    /// </summary>
    public class WCoreMvcStartup : IWCoreStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add MiniProfiler services
            services.AddWCoreMiniProfiler();

            //add WebMarkupMin services to the services container
            services.AddWCoreWebMarkupMin();

            //add and configure MVC feature
            services.AddWCoreMvc();

            //add custom redirect result executor
            services.AddWCoreRedirectResultExecutor();

        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use MiniProfiler
            application.UseMiniProfiler();

            //use WebMarkupMin
            application.UseWCoreWebMarkupMin();

            //Endpoints routing
            application.UseWCoreEndpoints();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 1000; //MVC should be loaded last
    }
}

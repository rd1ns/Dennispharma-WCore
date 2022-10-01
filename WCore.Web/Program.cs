using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WCore.Services.Configuration;

namespace WCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .ConfigureAppConfiguration(configuration => configuration.AddJsonFile(WCoreConfigurationDefaults.AppSettingsFilePath, true, true))
                    .UseIIS()
                    .CaptureStartupErrors(true).UseSetting("detailedErrors", "true")
                    .UseUrls("http://127.0.0.1:15900")
                    .UseStartup<Startup>()) 
                .Build()
                .Run();
        }
    }
}

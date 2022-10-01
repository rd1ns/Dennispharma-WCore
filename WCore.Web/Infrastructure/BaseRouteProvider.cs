using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using WCore.Core.Domain.Settings;
using WCore.Services.Localization;
using System.Linq;

namespace WCore.Web.Infrastructure
{
    public class BaseRouteProvider
    {
        protected string GetRouterPattern(IEndpointRouteBuilder endpointRouteBuilder, string seoCode = "")
        {
            var localizationSettings = endpointRouteBuilder.ServiceProvider.GetRequiredService<LocalizationSettings>();
            if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                var langservice = endpointRouteBuilder.ServiceProvider.GetRequiredService<ILanguageService>();
                var languages = langservice.GetAllLanguages().ToList();
                return "{language:lang=" + languages.FirstOrDefault().UniqueSeoCode + $"}}/{seoCode}";
            }
            return seoCode;
        }
    }
}

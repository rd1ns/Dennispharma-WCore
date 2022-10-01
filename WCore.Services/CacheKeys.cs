using WCore.Core.Caching;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Users;

namespace WCore.Services
{
    public class CacheKeys
    {
        public static class CountryKeys
        {
            static string CountryPrefix => "WCore.Country.all";
            public static CacheKey CountriesAllCacheKey => new CacheKey("WCore.Country.all.-{0}-{1}-{2}-{3}-{4}", CountryPrefix, WCoreEntityCacheDefaults<Country>.AllPrefix);
        }
        public static class CityKeys
        {
            static string CityPrefix => "WCore.City.all";
            public static CacheKey CitiesAllCacheKey => new CacheKey("WCore.City.all-{0}-{1}-{2}-{3}-{4}-{5}", CityPrefix, WCoreEntityCacheDefaults<City>.AllPrefix);
        }
        public static class DistrictKeys
        {
            static string DistrictPrefix => "WCore.District.all";
            public static CacheKey DistrictsAllCacheKey => new CacheKey("WCore.District.all-{0}-{1}-{2}-{3}-{4}-{5}-{6}}", DistrictPrefix, WCoreEntityCacheDefaults<City>.AllPrefix);
        }
        public static class DynamicFormKeys
        {
            static string DynamicFormPrefix => "WCore.DynamicForm.all";
            public static CacheKey DynamicFormsAllCacheKey => new CacheKey("WCore.DynamicForm.all-{0}-{1}-{2}-{3}-{4}-{5}", DynamicFormPrefix, WCoreEntityCacheDefaults<DynamicForm>.AllPrefix);
        }
        public static class DynamicFormElementKeys
        {
            static string DynamicFormElementPrefix => "WCore.DynamicFormElement.all";
            public static CacheKey DynamicFormElementsAllCacheKey => new CacheKey("WCore.DynamicFormElement.all-{0}", DynamicFormElementPrefix, WCoreEntityCacheDefaults<DynamicFormElement>.AllPrefix);
        }
        public static class DynamicFormRecordKeys
        {
            static string DynamicFormRecordPrefix => "WCore.DynamicFormRecord.all";
            public static CacheKey DynamicFormRecordsAllCacheKey => new CacheKey("WCore.DynamicFormRecord.all-{0}", DynamicFormRecordPrefix, WCoreEntityCacheDefaults<DynamicFormRecord>.AllPrefix);
        }
    }
}

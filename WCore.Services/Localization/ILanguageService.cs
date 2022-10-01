using WCore.Core;
using WCore.Core.Domain.Localization;
using System.Collections.Generic;

namespace WCore.Services.Localization
{
    /// <summary>
    /// Language service interface
    /// </summary>
    public partial interface ILanguageService : IRepository<Language>
    {

        /// <summary>
        /// Get 2 letter ISO language code
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>ISO language code</returns>
        string GetTwoLetterIsoLanguageName(Language language);

        /// <summary>
        /// Get admin default language
        /// </summary>
        /// <returns>Language</returns>
        Language GetAdminDefaultLanguage();

        /// <summary>
        /// Get default language
        /// </summary>
        /// <returns>Language</returns>
        Language GetDefaultLanguage();

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <returns>Languages</returns>
        List<Language> GetAllLanguages();
        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0);
        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        int GetAllCount();
        /// <summary>
        /// Get all language
        /// </summary>
        /// <returns>All language</returns>
        List<Language> GetAllLanguages(bool? AllowSelection = null, bool? Published = null);
        IPagedList<Language> GetAllByFilters(string searchValue = "", string sortColumnName = "", string sortColumnDirection = "", int skip = 0, int take = 10);
    }
}

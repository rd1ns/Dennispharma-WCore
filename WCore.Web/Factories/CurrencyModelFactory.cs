using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Core.Infrastructure;
using WCore.Services.Common;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Directory;

namespace WCore.Web.Factories
{
    public interface ICurrencyModelFactory
    {
        void PrepareCurrencyModel(CurrencyModel model, Currency entity);
        CurrencyModel PrepareCurrencyModel(int currencyId);
        CurrencyModel PrepareCurrencyModel(Currency entity);
        CurrencyListModel PrepareCurrencyListModel(CurrencyPagingFilteringModel command);
    }

    public class CurrencyModelFactory : ICurrencyModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ICurrencyService _currencyService;

        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        #endregion

        #region Methods
        public CurrencyModelFactory(UserSettings userSettings,
        ICurrencyService currencyService,
        ILocalizationService localizationService,
        IUserService userService,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        IStaticCacheManager staticCacheManager,
        IUrlRecordService urlRecordService,
        IWorkContext workContext,
        MediaSettings mediaSettings)
        {
            this._userSettings = userSettings;
            this._currencyService = currencyService;

            this._localizationService = localizationService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._genericAttributeService = genericAttributeService;
            this._staticCacheManager = staticCacheManager;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
        }
        #endregion

        public virtual CurrencyModel PrepareCurrencyModel(int currencyId)
        {
            if (currencyId == 0)
                throw new ArgumentNullException(nameof(currencyId));

            var entity = _currencyService.GetById(currencyId);

            return PrepareCurrencyModel(entity);
        }
        public virtual CurrencyModel PrepareCurrencyModel(Currency entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<CurrencyModel>();

            model.Name = _localizationService.GetLocalized(entity, x => x.Name);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">Currency post model</param>
        /// <param name="currency">Currency post entity</param>
        /// <param name="prepareComments">Whether to prepare Currency comments</param>
        public virtual void PrepareCurrencyModel(CurrencyModel model, Currency entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Name = _localizationService.GetLocalized(entity, x => x.Name);
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public CurrencyListModel PrepareCurrencyListModel(CurrencyPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new CurrencyListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;
            command.ShowOn = true;


            IPagedList<Currency> currencies = _currencyService.GetAllByFilters(searchValue: command.Query, Published: true,
                skip: command.PageNumber - 1,
                take: command.PageSize);

            model.PagingFilteringContext.LoadPagedList(currencies);

            model.Currencies = currencies
                .Select(x =>
                {
                    var entityModel = x.ToModel<CurrencyModel>();
                    PrepareCurrencyModel(entityModel, x);
                    return entityModel;
                }).ToList();

            return model;
        }
    }
}

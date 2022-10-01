using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Congresses;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Common;
using WCore.Services.Congresses;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Congresses;

namespace WCore.Web.Factories
{
    public interface ICongressPaperTypeModelFactory
    {
        void PrepareCongressPaperTypeModel(CongressPaperTypeModel model, CongressPaperType entity);
        CongressPaperTypeModel PrepareCongressPaperTypeModel(CongressPaperType entity);
        CongressPaperTypeListModel PrepareCongressPaperTypeListModel(CongressPaperTypePagingFilteringModel command);
    }

    public class CongressPaperTypeModelFactory : ICongressPaperTypeModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly ICongressPaperTypeService _congressPaperTypeService;

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
        public CongressPaperTypeModelFactory(UserSettings userSettings,
        ICongressPaperTypeService congressPaperTypeService,
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
            this._congressPaperTypeService = congressPaperTypeService;

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
        public virtual CongressPaperTypeModel PrepareCongressPaperTypeModel(CongressPaperType entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<CongressPaperTypeModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">CongressPaperType post model</param>
        /// <param name="congressPaperType">CongressPaperType post entity</param>
        /// <param name="prepareComments">Whether to prepare CongressPaperType comments</param>
        public virtual void PrepareCongressPaperTypeModel(CongressPaperTypeModel model, CongressPaperType entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public CongressPaperTypeListModel PrepareCongressPaperTypeListModel(CongressPaperTypePagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new CongressPaperTypeListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var congressPaperTypes = _congressPaperTypeService.GetAllByFilters(command.CongressId, command.Title, command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(congressPaperTypes);

            model.CongressPaperTypes = congressPaperTypes
                .Select(x =>
                {
                    var entityModel = x.ToModel<CongressPaperTypeModel>();
                    PrepareCongressPaperTypeModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

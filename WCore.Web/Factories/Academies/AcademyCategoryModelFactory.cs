using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Academies;
using WCore.Services.Common;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Academies;

namespace WCore.Web.Factories
{
    public interface IAcademyCategoryModelFactory
    {
        void PrepareAcademyCategoryModel(AcademyCategoryModel model, AcademyCategory entity);
        AcademyCategoryModel PrepareAcademyCategoryModel(AcademyCategory entity);
        AcademyCategoryListModel PrepareAcademyCategoryListModel(AcademyCategoryPagingFilteringModel command);
    }

    public class AcademyCategoryModelFactory : IAcademyCategoryModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IAcademyCategoryService _academyCategoryService;

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
        public AcademyCategoryModelFactory(UserSettings userSettings,
        IAcademyCategoryService academyCategoryService,
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
            this._academyCategoryService = academyCategoryService;

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
        public virtual AcademyCategoryModel PrepareAcademyCategoryModel(AcademyCategory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<AcademyCategoryModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">AcademyCategory post model</param>
        /// <param name="academyCategory">AcademyCategory post entity</param>
        /// <param name="prepareComments">Whether to prepare AcademyCategory comments</param>
        public virtual void PrepareAcademyCategoryModel(AcademyCategoryModel model, AcademyCategory entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public AcademyCategoryListModel PrepareAcademyCategoryListModel(AcademyCategoryPagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new AcademyCategoryListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            var academyCategories = _academyCategoryService.GetAllByFilters(command.ParentId, command.Title, command.IsActive, command.Deleted, command.ShowOn, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(academyCategories);

            model.AcademyCategories = academyCategories
                .Select(x =>
                {
                    var entityModel = x.ToModel<AcademyCategoryModel>();
                    PrepareAcademyCategoryModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }
    }
}

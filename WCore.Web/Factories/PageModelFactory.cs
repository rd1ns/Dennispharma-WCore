using System;
using System.Linq;
using WCore.Core;
using WCore.Core.Caching;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Settings;
using WCore.Core.Domain.Users;
using WCore.Services.Common;
using WCore.Services.Helpers;
using WCore.Services.Localization;
using WCore.Services.Pages;
using WCore.Services.Seo;
using WCore.Services.Users;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Pages;

namespace WCore.Web.Factories
{
    public interface IPageModelFactory
    {
        void PreparePageModel(PageModel model, Page entity);
        PageModel PreparePageModel(Page entity);
        PageModel PreparePageModel(bool homePage);
        PageListModel PreparePageListModel(PagePagingFilteringModel command);
    }

    public class PageModelFactory : IPageModelFactory
    {
        #region Fields
        private readonly UserSettings _userSettings;
        private readonly IPageService _pageService;
        private readonly INewsCategoryModelFactory _newsCategoryModelFactory;

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
        public PageModelFactory(UserSettings userSettings,
        IPageService pageService,
        INewsCategoryModelFactory newsCategoryModelFactory,
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
            this._pageService = pageService;
            this._newsCategoryModelFactory = newsCategoryModelFactory;

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
        public virtual PageModel PreparePageModel(Page entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var model = entity.ToModel<PageModel>();

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.ShortBody = _localizationService.GetLocalized(entity, x => x.ShortBody);

            model.MetaKeywords = _localizationService.GetLocalized(entity, x => x.MetaKeywords);
            model.MetaDescription = _localizationService.GetLocalized(entity, x => x.MetaDescription);
            model.MetaTitle = _localizationService.GetLocalized(entity, x => x.MetaTitle);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            return model;
        }

        /// <summary>
        /// Prepare ski resort model
        /// </summary>
        /// <param name="model">Page post model</param>
        /// <param name="page">Page post entity</param>
        /// <param name="prepareComments">Whether to prepare Page comments</param>
        public virtual void PreparePageModel(PageModel model, Page entity)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            model.Title = _localizationService.GetLocalized(entity, x => x.Title);
            model.Body = _localizationService.GetLocalized(entity, x => x.Body);
            model.ShortBody = _localizationService.GetLocalized(entity, x => x.ShortBody);

            model.MetaKeywords = _localizationService.GetLocalized(entity, x => x.MetaKeywords);
            model.MetaDescription = _localizationService.GetLocalized(entity, x => x.MetaDescription);
            model.MetaTitle = _localizationService.GetLocalized(entity, x => x.MetaTitle);

            model.SeName = _urlRecordService.GetSeName(entity, _workContext.WorkingLanguage.Id, ensureTwoPublishedLanguages: false);

            model.SubPages = _pageService.GetAllByFilters(ParentId: model.Id)
                .Select(x =>
                {
                    var entityModel = x.ToModel<PageModel>();
                    PreparePageModel(entityModel, x);
                    return entityModel;
                }).ToList();
        }
        /// <summary>
        /// Prepare ski resort list model
        /// </summary>
        /// <param name="command">Ski Resort paging filtering model</param>
        /// <returns>Ski resort list model</returns>
        public PageListModel PreparePageListModel(PagePagingFilteringModel command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new PageListModel
            {
                PagingFilteringContext = command,
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = 10;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            command.IsActive = true;
            command.Deleted = false;


            IPagedList<Page> pages = _pageService.GetAllByFilters(command.Query, command.PageType, command.FooterLocation, command.ParentId, command.IsActive, command.Deleted, command.ShowOn, command.HomePage, command.RedirectPage, command.PageNumber - 1, command.PageSize);


            model.PagingFilteringContext.LoadPagedList(pages);

            model.Pages = pages
                .Select(x =>
                {
                    var entityModel = x.ToModel<PageModel>();
                    PreparePageModel(entityModel, x);
                    return entityModel;
                })
                .ToList();
            return model;
        }

        public virtual PageModel PreparePageModel(bool homePage)
        {
            var page = _pageService.GetHomePage();
            var model = page.ToModel<PageModel>();

            PreparePageModel(model, page);
            return model;
        }

        //public List<PageModel> PagesWithBreadcrumb(List<PageModel> mlist = null, int parentId = 0, bool Sub = false)
        //{
        //    var pages = _pageService.GetAllByFilters(ParentId: parentId, ShowOn: true, Deleted: false, IsActive: true).OrderBy(o => o.DisplayOrder).ToList();
        //    if (Sub)
        //    {
        //        var newMenuList = new List<PageModel>();
        //        foreach (var page in pages)
        //        {
        //            newMenuList.Add(PreparePageModel(page));
        //        }
        //        return newMenuList;
        //    }
        //    else
        //    {
        //        foreach (var page in pages)
        //        {
        //            var slI = new MenuModel
        //            {
        //                Id = page.Id,
        //                MainPage = page.MainPage,
        //                Text = page.GetLocalized(o => o.Title),
        //                Url = page.GetSeName()
        //            };

        //            if (page.PageType == Core.Domain.Pages.PageType.News)
        //                slI.SubMenu.AddRange(PrepareBlogMenuModel());

        //            if (page.PageType == Core.Domain.Pages.PageType.Document)
        //                slI.SubMenu.AddRange(PrepareDocumentMenuModel());

        //            var subContent = _pageService.GetSubPageByParentId(page.Id).ToList();
        //            if (subContent.Count > 0)
        //            {
        //                slI.SubMenu.AddRange(PagesWithBreadcrumb(mlist, page.Id, true));
        //            }
        //            if (!Sub)
        //            {
        //                mlist.Add(slI);
        //            }
        //        }
        //        return mlist;
        //    }
        //}
    }
}

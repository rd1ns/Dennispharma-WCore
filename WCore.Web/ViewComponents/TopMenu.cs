using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WCore.Core;
using WCore.Services.Localization;
using WCore.Web.Factories;
using WCore.Web.Models.Academies;
using WCore.Web.Models.Congresses;
using WCore.Web.Models.Newses;
using WCore.Web.Models.Pages;
using WCore.Web.Models.Teams;

namespace WCore.Web.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        private readonly IPageModelFactory _pageModelFactory;
        private readonly INewsCategoryModelFactory _newsCategoryModelFactory;
        private readonly ITeamCategoryModelFactory _teamCategoryModelFactory;
        private readonly IAcademyCategoryModelFactory _academyCategoryModelFactory;
        private readonly ICongressModelFactory _congressModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public TopMenuViewComponent(IPageModelFactory pageModelFactory,
            INewsCategoryModelFactory newsCategoryModelFactory,
            ITeamCategoryModelFactory teamCategoryModelFactory,
            IAcademyCategoryModelFactory academyCategoryModelFactory,
            ICongressModelFactory congressModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._pageModelFactory = pageModelFactory;
            this._newsCategoryModelFactory = newsCategoryModelFactory;
            this._teamCategoryModelFactory = teamCategoryModelFactory;
            this._academyCategoryModelFactory = academyCategoryModelFactory;
            this._congressModelFactory = congressModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke()
        {
            var pages = _pageModelFactory.PreparePageListModel(new Models.Pages.PagePagingFilteringModel() { ParentId = 0, IsActive = true, Deleted = false, ShowOn = true, PageSize = int.MaxValue, WithEntityPages = true });
            pages.Pages = FindEntityPages(pages.Pages);
            return View(pages);
        }

        public List<PageModel> FindEntityPages(List<PageModel> pages)
        {
            foreach (var page in pages)
            {
                switch (page.PageType)
                {
                    case Core.Domain.Pages.PageType.News:
                        var newsCategories = _newsCategoryModelFactory.PrepareNewsCategoryListModel(new NewsCategoryPagingFilteringModel()
                        {
                            ShowOn = true,
                            IsActive = true,
                            Deleted = false
                        }).NewsCategories.Select(o =>
                        {
                            var p = new PageModel();
                            p.Title = o.Title;
                            p.SeName = o.SeName;
                            return p;
                        });
                        page.SubPages.AddRange(newsCategories);
                        break;
                    case Core.Domain.Pages.PageType.Teams:
                        var teamCategories = _teamCategoryModelFactory.PrepareTeamCategoryListModel(new TeamCategoryPagingFilteringModel()
                        {
                            ShowOn = true,
                            IsActive = true,
                            Deleted = false,
                            ParentId = 0,
                            PageSize = int.MaxValue
                        }).TeamCategories.Select(o =>
                        {
                            var p = new PageModel();
                            p.Title = o.Title;
                            p.SeName = o.SeName;
                            p.SubPages = FindTeamCategoryPages(o.SubTeamCategories);
                            return p;
                        });
                        page.SubPages.AddRange(teamCategories);
                        break;
                    case Core.Domain.Pages.PageType.Academy:
                        var academyCategories = _academyCategoryModelFactory.PrepareAcademyCategoryListModel(new AcademyCategoryPagingFilteringModel()
                        {
                            ShowOn = true,
                            IsActive = true,
                            Deleted = false,
                            PageSize = int.MaxValue
                        }).AcademyCategories.Select(o =>
                        {
                            var p = new PageModel();
                            p.Title = o.Title;
                            p.SeName = o.SeName;
                            return p;
                        });
                        page.SubPages.AddRange(academyCategories);
                        break;
                    case Core.Domain.Pages.PageType.Congress:
                        var congress = _congressModelFactory.PrepareCongressListModel(new CongressPagingFilteringModel()
                        {
                            ShowOn = true,
                            IsActive = true,
                            Deleted = false,
                            PageSize = int.MaxValue
                        }).Congresses.Select(o =>
                        {
                            var p = new PageModel();
                            p.Title = o.Title;
                            p.SeName = o.SeName;
                            return p;
                        });
                        page.SubPages.AddRange(congress);
                        break;
                }
                if (page.SubPages.Any())
                {
                    FindEntityPages(page.SubPages);
                }
            }
            return pages;
        }

        public List<PageModel> FindTeamCategoryPages(List<TeamCategoryModel> teamCategories)
        {
            var pages = teamCategories.Select(o =>
            {
                var p = new PageModel();
                p.Title = o.Title;
                p.SeName = o.SeName;
                p.SubPages = FindTeamCategoryPages(o.SubTeamCategories);
                return p;
            }).ToList();
            return pages;
        }
    }
}

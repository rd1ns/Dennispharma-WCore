using Microsoft.AspNetCore.Mvc;
using WCore.Core;
using WCore.Services.Academies;
using WCore.Services.Localization;
using WCore.Services.Users;
using WCore.Web.Factories;
using WCore.Web.Infrastructure.Mapper;
using WCore.Web.Models.Academies;

namespace WCore.Web.ViewComponents
{
    public class AcademyCategoryViewComponent : ViewComponent
    {
        private readonly IAcademyCategoryService _academyCategoryService;
        private readonly IAcademyCategoryModelFactory _academyCategoryModelFactory;
        private readonly IAcademyService _academyService;
        private readonly IAcademyModelFactory _academyModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        public AcademyCategoryViewComponent(IAcademyCategoryService academyCategoryService,
            IAcademyCategoryModelFactory academyCategoryModelFactory,
            IAcademyService academyService,
            IAcademyModelFactory academyModelFactory,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._academyCategoryService = academyCategoryService;
            this._academyCategoryModelFactory = academyCategoryModelFactory;
            this._academyService = academyService;
            this._academyModelFactory = academyModelFactory;
            this._localizationService = localizationService;

            this._workContext = workContext;
        }
        public IViewComponentResult Invoke(int academyCategoryId)
        {
            if (academyCategoryId == 0)
            {
                var academyCategories = _academyCategoryModelFactory.PrepareAcademyCategoryListModel(new AcademyCategoryPagingFilteringModel(){ });

                foreach (var _academyCategory in academyCategories.AcademyCategories)
                {
                    var _academyPaging = new AcademyPagingFilteringModel()
                    {
                        AcademyCategoryId = _academyCategory.Id
                    };
                    _academyCategory.Academies = _academyModelFactory.PrepareAcademyListModel(_academyPaging);
                }
                return View(academyCategories);

            }
            var academyCategory = _academyCategoryService.GetById(academyCategoryId);

            var model = academyCategory.ToModel<AcademyCategoryModel>();

            _academyCategoryModelFactory.PrepareAcademyCategoryModel(model, academyCategory);


            var academyPaging = new AcademyPagingFilteringModel()
            {
                AcademyCategoryId = academyCategoryId
            };
            model.Academies = _academyModelFactory.PrepareAcademyListModel(academyPaging);

            return View(model);
        }
    }
}

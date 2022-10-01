using AutoMapper;
using WCore.Core.Domain.Academies;
using WCore.Core.Domain.Common;
using WCore.Core.Domain.Congresses;
using WCore.Core.Domain.Directory;
using WCore.Core.Domain.DynamicForms;
using WCore.Core.Domain.Galleries;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Newses;
using WCore.Core.Domain.Orders;
using WCore.Core.Domain.Pages;
using WCore.Core.Domain.Teams;
using WCore.Core.Domain.Users;
using WCore.Core.Infrastructure.Mapper;
using WCore.Web.Models.Academies;
using WCore.Web.Models.Common;
using WCore.Web.Models.Congresses;
using WCore.Web.Models.Directory;
using WCore.Web.Models.DynamicForms;
using WCore.Web.Models.Galleries;
using WCore.Web.Models.Localization;
using WCore.Web.Models.Newses;
using WCore.Web.Models.Pages;
using WCore.Web.Models.Teams;
using WCore.Web.Models.Users;

namespace WCore.Web.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class WCoreMapperConfiguration : Profile, IOrderedMapperProfile
    {

        public WCoreMapperConfiguration()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<UserRegistrationForm, UserRegistrationFormModel>();
            CreateMap<UserRegistrationFormModel, UserRegistrationForm>();

            CreateMap<Language, LanguageModel>();
            CreateMap<LanguageModel, Language>();

            CreateMap<Currency, CurrencyModel>();
            CreateMap<CurrencyModel, Currency>();

            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            CreateMap<City, CityModel>();
            CreateMap<CityModel, City>();

            CreateMap<Page, PageModel>();
            CreateMap<PageModel, Page>();

            CreateMap<Gallery, GalleryModel>();
            CreateMap<GalleryModel, Gallery>();

            CreateMap<GalleryImage, GalleryImageModel>();
            CreateMap<GalleryImageModel, GalleryImage>();

            CreateMap<DynamicForm, DynamicFormModel>();
            CreateMap<DynamicFormModel, DynamicForm>();

            CreateMap<DynamicFormElement, DynamicFormElementModel>();
            CreateMap<DynamicFormElementModel, DynamicFormElement>();

            CreateMap<DynamicFormRecord, DynamicFormRecordModel>();
            CreateMap<DynamicFormRecordModel, DynamicFormRecord>();

            //Congress
            CreateMap<CongressImage, CongressImageModel>();
            CreateMap<CongressImageModel, CongressImage>();

            CreateMap<Congress, CongressModel>();
            CreateMap<CongressModel, Congress>();

            CreateMap<CongressPaper, CongressPaperModel>();
            CreateMap<CongressPaperModel, CongressPaper>();

            CreateMap<CongressPaperType, CongressPaperTypeModel>();
            CreateMap<CongressPaperTypeModel, CongressPaperType>();

            CreateMap<CongressPresentation, CongressPresentationModel>();
            CreateMap<CongressPresentationModel, CongressPresentation>();

            CreateMap<CongressPresentationType, CongressPresentationTypeModel>();
            CreateMap<CongressPresentationTypeModel, CongressPresentationType>();

            //News
            CreateMap<NewsImage, NewsImageModel>();
            CreateMap<NewsImageModel, NewsImage>();

            CreateMap<News, NewsModel>();
            CreateMap<NewsModel, News>();

            CreateMap<NewsCategory, NewsCategoryModel>();
            CreateMap<NewsCategoryModel, NewsCategory>();

            //Team
            CreateMap<Team, TeamModel>();
            CreateMap<TeamModel, Team>();

            CreateMap<TeamCategory, TeamCategoryModel>();
            CreateMap<TeamCategoryModel, TeamCategory>();

            //Academy
            CreateMap<Academy, AcademyModel>();
            CreateMap<AcademyModel, Academy>();

            CreateMap<AcademyCategory, AcademyCategoryModel>();
            CreateMap<AcademyCategoryModel, AcademyCategory>();

            CreateMap<AcademyImage, AcademyImageModel>();
            CreateMap<AcademyImageModel, AcademyImage>();

            CreateMap<AcademyFile, AcademyFileModel>();
            CreateMap<AcademyFileModel, AcademyFile>();

            CreateMap<AcademyVideo, AcademyVideoModel>();
            CreateMap<AcademyVideoModel, AcademyVideo>();
        }

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
}
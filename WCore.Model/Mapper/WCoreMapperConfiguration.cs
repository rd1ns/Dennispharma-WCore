using AutoMapper;
using SkiTurkish.Core.Domain.Common;
using SkiTurkish.Core.Domain.Users;
using SkiTurkish.Core.Infrastructure.Mapper;
using SkiTurkish.Model.Common;
using SkiTurkish.Model.Users;

namespace SkiTurkish.Model.Mapper
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class SkiTurkishMapperConfiguration : Profile, IOrderedMapperProfile
    {

        public SkiTurkishMapperConfiguration()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();

            CreateMap<City, CityModel>();
            CreateMap<CityModel, City>();

            CreateMap<District, DistrictModel>();
            CreateMap<DistrictModel, District>();


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
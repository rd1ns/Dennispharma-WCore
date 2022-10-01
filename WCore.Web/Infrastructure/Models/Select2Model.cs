using WCore.Web.Areas.Admin.Models.Common;
using WCore.Web.Areas.Admin.Models.Users;
using System.Collections.Generic;

namespace WCore.Web.Models
{
    public class Select2_CountryModel
    {
        public bool incomplate_results { get; set; }
        public List<CountryModel> items { get; set; }
        public int total_count { get; set; }
    }
    public class Select2_CityModel
    {
        public bool incomplate_results { get; set; }
        public List<CityModel> items { get; set; }
        public int total_count { get; set; }
    }
    public class Select2_DistrictModel
    {
        public bool incomplate_results { get; set; }
        public List<DistrictModel> items { get; set; }
        public int total_count { get; set; }
    }
    public class Select2_UserModel
    {
        public bool incomplate_results { get; set; }
        public List<UserModel> items { get; set; }
        public int total_count { get; set; }
    }
}

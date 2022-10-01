using SkiTurkish.Model.Common;
using System.Collections.Generic;

namespace SkiTurkish.Model.GridModels
{
    public class CountryGridModel
    {
        public CountryGridModel()
        {
            Data = new List<CountryModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CountryModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
    public class CityGridModel
    {
        public CityGridModel()
        {
            Data = new List<CityModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CityModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
    public class DistrictGridModel
    {
        public DistrictGridModel()
        {
            Data = new List<DistrictModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<DistrictModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
}

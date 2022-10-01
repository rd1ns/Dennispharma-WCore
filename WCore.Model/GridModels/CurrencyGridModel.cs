using SkiTurkish.Model.Directory;
using System.Collections.Generic;

namespace SkiTurkish.Model.GridModels
{
    public class CurrencyGridModel
    {
        public CurrencyGridModel()
        {
            Data = new List<CurrencyModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CurrencyModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
}

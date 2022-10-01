using SkiTurkish.Model.Localization;
using System.Collections.Generic;

namespace SkiTurkish.Model.GridModels
{
    public class LanguageGridModel
    {
        public LanguageGridModel()
        {
            Data = new List<LanguageModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<LanguageModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
    public class LocalizedPropertyGridModel
    {
        public LocalizedPropertyGridModel()
        {
            Data = new List<LocalizedPropertyModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<LocalizedPropertyModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
    public class LocaleStringResourceGridModel
    {
        public LocaleStringResourceGridModel()
        {
            Data = new List<LocaleStringResourceModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<LocaleStringResourceModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
}

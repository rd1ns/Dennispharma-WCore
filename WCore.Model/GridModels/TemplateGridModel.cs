using System.Collections.Generic;
using SkiTurkish.Model.Templates;

namespace SkiTurkish.Model.GridModels
{
    public class TemplateGridModel
    {
        public TemplateGridModel()
        {
            Data = new List<TemplateModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<TemplateModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
}

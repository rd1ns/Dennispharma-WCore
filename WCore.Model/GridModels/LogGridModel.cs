using System.Collections.Generic;
using SkiTurkish.Core.Domain.Logging;

namespace SkiTurkish.Model.GridModels
{
    public class LogGridModel
    {
        public LogGridModel()
        {
            Data = new List<Log>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<Log> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
}

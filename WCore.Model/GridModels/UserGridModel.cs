using System.Collections.Generic;
using SkiTurkish.Core.Domain.Users;
using SkiTurkish.Model.Users;

namespace SkiTurkish.Model.GridModels
{
    public class UserGridModel
    {
        public UserGridModel()
        {
            Data = new List<UserModel>();
        }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<UserModel> Data { get; set; }
        public GridMetaModel Meta { get; set; }
    }
}

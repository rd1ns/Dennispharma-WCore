using System;
using WCore.Core.Domain.Localization;
using WCore.Core.Domain.Seo;

namespace WCore.Core.Domain.Congresses
{    public partial class CongressPaperType : BaseEntity, ILocalizedEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public int CongressId { get; set; }
        public Congress Congress { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
    }
}

using WCore.Framework.Models;

namespace WCore.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a templates model
    /// </summary>
    public partial class TemplatesModel : BaseWCoreModel
    {
        #region Ctor

        public TemplatesModel()
        {
            TemplatesCategory = new CategoryTemplateSearchModel();
            TemplatesManufacturer = new ManufacturerTemplateSearchModel();
            TemplatesProduct = new ProductTemplateSearchModel();

            AddCategoryTemplate = new CategoryTemplateModel();
            AddManufacturerTemplate = new ManufacturerTemplateModel();
            AddProductTemplate = new ProductTemplateModel();
        }

        #endregion

        #region Properties

        public CategoryTemplateSearchModel TemplatesCategory { get; set; }

        public ManufacturerTemplateSearchModel TemplatesManufacturer { get; set; }

        public ProductTemplateSearchModel TemplatesProduct { get; set; }

        public CategoryTemplateModel AddCategoryTemplate { get; set; }

        public ManufacturerTemplateModel AddManufacturerTemplate { get; set; }

        public ProductTemplateModel AddProductTemplate { get; set; }

        #endregion
    }
}

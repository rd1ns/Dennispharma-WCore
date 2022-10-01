namespace WCore.Framework.Models
{
    /// <summary>
    /// Represents base wCoreCommerce entity model
    /// </summary>
    public partial class BaseWCoreEntityModel : BaseWCoreModel
    {
        /// <summary>
        /// Gets or sets model identifier
        /// </summary>
        public virtual int Id { get; set; }
    }
}

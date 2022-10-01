namespace WCore.Core.Domain
{
    /// <summary>
    /// Base class with generic type identifier
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class BaseEntityTypeId<TId>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual TId Id { get; set; }
    }
}
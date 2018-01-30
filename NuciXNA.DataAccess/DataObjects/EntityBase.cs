namespace NuciXNA.DataAccess.DataObjects
{
    public class EntityBase : EntityBase<string> { }

    public class EntityBase<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TKey Id { get; set; }
    }
}

using System.Collections.Generic;

using NuciXNA.DataAccess.DataObjects;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// Repository interface.
    /// </summary>
    public interface IRepository<TDataObject> : IRepository<string, TDataObject> where TDataObject : EntityBase
    {
    }

    /// <summary>
    /// Repository interface.
    /// </summary>
    public interface IRepository<TKey, TDataObject> where TDataObject : EntityBase<TKey>
    {
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Add(TDataObject entity);

        /// <summary>
        /// Gets the entity  with the specified identifier.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="id">Identifier.</param>
        TDataObject Get(TKey id);

        /// <summary>
        /// Gets all the entities.
        /// </summary>
        /// <returns>The entities.</returns>
        IEnumerable<TDataObject> GetAll();

        /// <summary>
        /// Updates the specified entity's fields.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Update(TDataObject entity);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Remove(TDataObject entity);

        /// <summary>
        /// Removes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Remove(TKey id);
    }
}

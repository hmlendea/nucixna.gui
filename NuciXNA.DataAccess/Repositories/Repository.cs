using System.Collections.Generic;
using System.Linq;

using NuciXNA.DataAccess.DataObjects;
using NuciXNA.DataAccess.Exceptions;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// XML-based repository.
    /// </summary>
    public abstract class Repository<TDataObject> : IRepository<string, TDataObject> where TDataObject : EntityBase
    {
        /// <summary>
        /// The stored entities.
        /// </summary>
        protected List<TDataObject> Entities;
        
        bool loadedEntities;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Repository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public Repository(string fileName)
        {
            Entities = new List<TDataObject>();
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Add(TDataObject entity)
        {
            Entities.Add(entity);
        }

        /// <summary>
        /// Get the entity with the specified identifier.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="id">Identifier.</param>
        public virtual TDataObject Get(string id)
        {
            TDataObject entity = Entities.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException(id, nameof(TDataObject));
            }

            return entity;
        }

        /// <summary>
        /// Gets all the entities.
        /// </summary>
        /// <returns>The entities</returns>
        public virtual IEnumerable<TDataObject> GetAll()
        {
            return Entities;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public abstract void Update(TDataObject entity);

        /// <summary>
        /// Removes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual void Remove(string id)
        {
            Entities.RemoveAll(x => x.Id == id);
        }
    }
}

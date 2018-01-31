using System.Collections.Generic;
using System.Linq;

using NuciXNA.DataAccess.DataObjects;
using NuciXNA.DataAccess.Exceptions;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// In-memory repository.
    /// </summary>
    public abstract class Repository<TDataObject> : Repository<string, TDataObject> where TDataObject : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Repository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public Repository(string fileName) : base(fileName) { }
    }

    /// <summary>
    /// In-memory repository.
    /// </summary>
    public abstract class Repository<TKey, TDataObject> : IRepository<TKey, TDataObject> where TDataObject : EntityBase<TKey>
    {
        /// <summary>
        /// The stored entities.
        /// </summary>
        protected List<TDataObject> Entities;

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
        public virtual TDataObject Get(TKey id)
        {
            TDataObject entity = Entities.FirstOrDefault(x => x.Id.Equals(id));

            if (entity == null)
            {
                throw new EntityNotFoundException(id.ToString(), nameof(TDataObject));
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
        /// Updates the specified entity's fields.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public abstract void Update(TDataObject entity);
        
        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public virtual void Remove(TDataObject entity)
        {
            Entities.Remove(entity);
        }

        /// <summary>
        /// Removes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public void Remove(TKey id)
        {
            TDataObject entity = Get(id);

            Remove(entity);
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;

using NuciXNA.DataAccess.DataObjects;
using NuciXNA.DataAccess.Exceptions;
using NuciXNA.DataAccess.IO;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// XML-based repository.
    /// </summary>
    public abstract class XmlRepository<TDataObject> : IRepository<string, TDataObject> where TDataObject : EntityBase
    {
        /// <summary>
        /// The stored entities.
        /// </summary>
        protected List<TDataObject> Entities;

        /// <summary>
        /// The XML file.
        /// </summary>
        protected readonly XmlFileCollection<TDataObject> XmlFile;

        bool loadedEntities;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XmlRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public XmlRepository(string fileName)
        {
            XmlFile = new XmlFileCollection<TDataObject>(fileName);
            Entities = new List<TDataObject>();
        }

        public void ApplyChanges()
        {
            try
            {
                XmlFile.SaveEntities(Entities);
            }
            catch
            {
                // TODO: Better exception message
                throw new IOException("Cannot save the changes");
            }
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public void Add(TDataObject entity)
        {
            LoadEntitiesIfNeeded();

            Entities.Add(entity);
        }

        /// <summary>
        /// Get the entity with the specified identifier.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="id">Identifier.</param>
        public TDataObject Get(string id)
        {
            LoadEntitiesIfNeeded();

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
        public IEnumerable<TDataObject> GetAll()
        {
            LoadEntitiesIfNeeded();

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
        public void Remove(string id)
        {
            LoadEntitiesIfNeeded();

            Entities.RemoveAll(x => x.Id == id);

            try
            {
                XmlFile.SaveEntities(Entities);
            }
            catch
            {
                throw new DuplicateEntityException(id, nameof(TDataObject));
            }
        }

        /// <summary>
        /// Loads the entities if needed.
        /// </summary>
        protected void LoadEntitiesIfNeeded()
        {
            if (loadedEntities)
            {
                return;
            }

            Entities = XmlFile.LoadEntities().ToList();
            loadedEntities = true;
        }
    }
}

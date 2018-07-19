using System.Collections.Generic;
using System.IO;

using NuciXNA.DataAccess.DataObjects;
using NuciXNA.DataAccess.IO;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// XML-based repository.
    /// </summary>
    public abstract class XmlRepository<TDataObject> : XmlRepository<string, TDataObject> where TDataObject : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:XmlRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public XmlRepository(string fileName) : base(fileName) { }
    }

    /// <summary>
    /// XML-based repository.
    /// </summary>
    public abstract class XmlRepository<TKey, TDataObject> : Repository<TKey, TDataObject> where TDataObject : EntityBase<TKey>
    {
        /// <summary>
        /// The XML file.
        /// </summary>
        protected readonly XmlFileCollection<TDataObject> XmlFile;

        bool loadedEntities;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XmlRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public XmlRepository(string fileName) : base()
        {
            XmlFile = new XmlFileCollection<TDataObject>(fileName);
        }

        public void ApplyChanges()
        {
            try
            {
                XmlFile.SaveEntities(Entities.Values);
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
        public override void Add(TDataObject entity)
        {
            if (!loadedEntities)
            {
                LoadEntitiesIfNeeded();
            }
            
            base.Add(entity);
        }

        /// <summary>
        /// Get the entity with the specified identifier.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="id">Identifier.</param>
        public override TDataObject Get(TKey id)
        {
            if (!loadedEntities)
            {
                LoadEntitiesIfNeeded();
            }

            return base.Get(id);
        }

        /// <summary>
        /// Gets all the entities.
        /// </summary>
        /// <returns>The entities</returns>
        public override IEnumerable<TDataObject> GetAll()
        {
            if (!loadedEntities)
            {
                LoadEntitiesIfNeeded();
            }

            return base.GetAll();
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public override void Remove(TDataObject entity)
        {
            if (!loadedEntities)
            {
                LoadEntitiesIfNeeded();
            }

            base.Remove(entity);

            try
            {
                XmlFile.SaveEntities(Entities.Values);
            }
            catch
            {
                throw new DuplicateEntityException(entity.Id.ToString(), nameof(TDataObject));
            }
        }

        /// <summary>
        /// Loads the entities if needed.
        /// </summary>
        protected void LoadEntitiesIfNeeded()
        {
            IEnumerable<TDataObject> xmlEntities = XmlFile.LoadEntities();

            foreach(TDataObject entity in xmlEntities)
            {
                if (Entities.ContainsKey(entity.Id))
                {
                    throw new DuplicateEntityException(entity.Id.ToString(), nameof(TDataObject));
                }

                Entities.Add(entity.Id, entity);
            }

            loadedEntities = true;
        }
    }
}

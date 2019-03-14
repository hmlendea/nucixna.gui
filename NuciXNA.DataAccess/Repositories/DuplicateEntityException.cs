using System;

using NuciXNA.DataAccess.DataObjects;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    public class DuplicateEntityException : EntityException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> exception.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public DuplicateEntityException(EntityBase entity)
            : base(entity, $"The {entity.Id} {entity.GetType().Name} entity is duplicated.")
        {
            
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public DuplicateEntityException(string entityId, Type entityType)
            : base(entityId, entityType, $"The {entityId} {entityType.Name} entity is duplicated.")
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public DuplicateEntityException(string entityId, string entityType)
            : base(entityId, entityType, $"The {entityId} {entityType} entity is duplicated.")
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> exception.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(EntityBase entity, Exception innerException)
            : base(entity, $"The {entity.Id} {entity.GetType().Name} entity is duplicated.", innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(string entityId, Type entityType, Exception innerException)
            : base(entityId, entityType, $"The {entityId} {entityType.Name} entity is duplicated.", innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(string entityId, string entityType, Exception innerException)
            : base(entityId, entityType, $"The {entityId} {entityType} entity is duplicated.", innerException)
        {
            
        }
    }
}

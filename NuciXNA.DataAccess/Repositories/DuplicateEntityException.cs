using System;

using NuciXNA.DataAccess.DataObjects;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    public class DuplicateEntityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public DuplicateEntityException(EntityBase entity)
            : base($"The {entity.Id} {entity.GetType().Name} entity is duplicated.")
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public DuplicateEntityException(string entityId, Type entityType)
            : base($"The {entityId} {entityType.Name} entity is duplicated.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public DuplicateEntityException(string entityId, string entityType)
            : base($"The {entityId} {entityType} entity is duplicated.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(EntityBase entity, Exception innerException)
            : base($"The {entity.Id} {entity.GetType().Name} entity is duplicated.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(string entityId, Type entityType, Exception innerException)
            : base($"The {entityId} {entityType.Name} entity is duplicated.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEntityException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public DuplicateEntityException(string entityId, string entityType, Exception innerException)
            : base($"The {entityId} {entityType} entity is duplicated.", innerException)
        {
        }
    }
}

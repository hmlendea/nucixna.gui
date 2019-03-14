using System;

using NuciXNA.DataAccess.DataObjects;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// Repository exception.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityNotFoundException(EntityBase entity)
            : base($"The {entity.Id} {entity.GetType().Name} entity can not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public EntityNotFoundException(string entityId, Type entityType)
            : base($"The {entityId} {entityType.Name} entity can not be found.")
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public EntityNotFoundException(string entityId, string entityType)
            : base($"The {entityId} {entityType} entity can not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(EntityBase entity, Exception innerException)
            : base($"The {entity.Id} {entity.GetType().Name} entity can not be found.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(string entityId, Type entityType, Exception innerException)
            : base($"The {entityId} {entityType.Name} entity can not be found.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(string entityId, string entityType, Exception innerException)
            : base($"The {entityId} {entityType} entity can not be found.", innerException)
        {
        }
    }
}

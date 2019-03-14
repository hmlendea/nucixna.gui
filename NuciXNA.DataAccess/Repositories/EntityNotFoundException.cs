using System;

using NuciXNA.DataAccess.DataObjects;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// Repository exception.
    /// </summary>
    public class EntityNotFoundException : EntityException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> exception.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityNotFoundException(EntityBase entity)
            : base(entity, $"The {entity.Id} {entity.GetType().Name} entity can not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public EntityNotFoundException(string entityId, Type entityType)
            : base(entityId, entityType, $"The {entityId} {entityType.Name} entity can not be found.")
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public EntityNotFoundException(string entityId, string entityType)
            : base(entityId, entityType, $"The {entityId} {entityType} entity can not be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> exception.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(EntityBase entity, Exception innerException)
            : base(entity, $"The {entity.Id} {entity.GetType().Name} entity can not be found.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(string entityId, Type entityType, Exception innerException)
            : base(entityId, entityType, $"The {entityId} {entityType.Name} entity can not be found.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityNotFoundException(string entityId, string entityType, Exception innerException)
            : base(entityId, entityType, $"The {entityId} {entityType} entity can not be found.", innerException)
        {
        }
    }
}

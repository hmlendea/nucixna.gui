using System;

using NuciXNA.DataAccess.DataObjects;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    public abstract class EntityException : Exception
    {
        public string EntityId { get; }

        public string EntityTypeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> exception.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The exception message.</param>
        public EntityException(EntityBase entity, string message)
            : base(message)
        {
            EntityId = entity.Id;
            EntityTypeName = entity.GetType().Name;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="message">The exception message.</param>
        public EntityException(string entityId, Type entityType, string message)
            : base(message)
        {
            EntityId = entityId;
            EntityTypeName = entityType.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="message">The exception message.</param>
        public EntityException(string entityId, string entityType, string message)
            : base(message)
        {
            EntityId = entityId;
            EntityTypeName = entityType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> exception.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityException(EntityBase entity, string message, Exception innerException)
            : base(message, innerException)
        {
            EntityId = entity.Id;
            EntityTypeName = entity.GetType().Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityException(string entityId, Type entityType, string message, Exception innerException)
            : base(message, innerException)
        {
            EntityId = entityId;
            EntityTypeName = entityType.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityException"/> exception.
        /// </summary>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public EntityException(string entityId, string entityType, string message, Exception innerException)
            : base(message, innerException)
        {
            EntityId = entityId;
            EntityTypeName = entityType;
        }
    }
}

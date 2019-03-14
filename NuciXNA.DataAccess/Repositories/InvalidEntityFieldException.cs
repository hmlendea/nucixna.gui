using System;

using NuciXNA.DataAccess.DataObjects;

namespace NuciXNA.DataAccess.Repositories
{
    /// <summary>
    /// Duplicate entity exception.
    /// </summary>
    public class InvalidEntityFieldException : EntityException
    {
        public string FieldName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityFieldException"/> exception.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entity">The entity.</param>
        public InvalidEntityFieldException(string fieldName, EntityBase entity)
            : base(entity, $"The {fieldName} field of {entity.Id} {entity.GetType().Name} entity is invalid.")
        {
            FieldName = fieldName;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityFieldException"/> exception.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public InvalidEntityFieldException(string fieldName, string entityId, Type entityType)
            : base(entityId, entityType, $"The {fieldName} field of {entityId} {entityType.Name} entity is invalid.")
        {
            FieldName = fieldName;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityFieldException"/> exception.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        public InvalidEntityFieldException(string fieldName, string entityId, string entityType)
            : base(entityId, entityType, $"The {fieldName} field of {entityId} {entityType} entity is invalid.")
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityFieldException"/> exception.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidEntityFieldException(string fieldName, EntityBase entity, Exception innerException)
            : base(entity, $"The {fieldName} field of {entity.Id} {entity.GetType().Name} entity is invalid.", innerException)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityFieldException"/> exception.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidEntityFieldException(string fieldName, string entityId, Type entityType, Exception innerException)
            : base(entityId, entityType, $"The {fieldName} field of {entityId} {entityType.Name} entity is invalid.", innerException)
        {
            FieldName = fieldName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEntityFieldException"/> exception.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="entityId">Entity identifier.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidEntityFieldException(string fieldName, string entityId, string entityType, Exception innerException)
            : base(entityId, entityType, $"The {fieldName} field of {entityId} {entityType} entity is invalid.", innerException)
        {
            FieldName = fieldName;
        }
    }
}

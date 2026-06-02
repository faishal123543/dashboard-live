namespace Fintech.Backoffice.Domain.Common
{
    /// <summary>
    /// Base entity class that all domain entities inherit from.
    /// Provides common properties like Id, CreatedDate, and ModifiedDate.
    /// Why: Following DDD (Domain Driven Design) principles, having a base class for all entities
    /// ensures consistency and provides audit trail capabilities across the domain.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Primary key identifier for the entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time when the entity was created.
        /// Automatically set when entity is first saved.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the entity was last modified.
        /// Updated every time the entity is saved.
        /// </summary>
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User ID who created this entity (optional, set by application).
        /// Useful for audit trail.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User ID who last modified this entity (optional, set by application).
        /// Useful for audit trail.
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}

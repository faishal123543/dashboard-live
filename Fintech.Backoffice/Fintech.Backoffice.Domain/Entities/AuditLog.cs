using Fintech.Backoffice.Domain.Common;

namespace Fintech.Backoffice.Domain.Entities
{
    /// <summary>
    /// Represents an audit log entry tracking changes to important entities.
    /// Provides a complete audit trail for compliance and regulatory requirements.
    /// Why: Financial applications require complete audit trails showing who changed what and when.
    /// This is essential for SOX, PCI-DSS, and other compliance standards.
    /// </summary>
    public class AuditLog : BaseEntity
    {
        /// <summary>
        /// Name of the entity that was changed.
        /// Example: "Application", "Customer", "Partner"
        /// This allows flexible auditing of multiple entities.
        /// </summary>
        public string EntityName { get; set; } = string.Empty;

        /// <summary>
        /// Primary key ID of the entity that was changed.
        /// Combined with EntityName, uniquely identifies which record was changed.
        /// Example: If EntityName="Application", EntityId=123 identifies Application with ID 123.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// The action performed: "Create", "Update", "Delete", "Approve", "Reject", etc.
        /// Allows tracking specific business operations.
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// JSON string representation of the old values before the change.
        /// Example: {"Status":"InProgress", "ApprovedAmount":0}
        /// Why: JSON format allows flexible storage of different entity structures.
        /// Stored as string to avoid database schema changes when entity properties change.
        /// </summary>
        public string? OldValues { get; set; }

        /// <summary>
        /// JSON string representation of the new values after the change.
        /// Example: {"Status":"Completed", "ApprovedAmount":50000}
        /// Allows tracking exactly what changed and what the new values are.
        /// </summary>
        public string? NewValues { get; set; }

        /// <summary>
        /// User ID of the person who made the change.
        /// Stored as string to support various user ID formats (GUID, string, etc.).
        /// Essential for "who did what" audit requirements.
        /// </summary>
        public string? ChangedBy { get; set; }

        /// <summary>
        /// IP address from which the change was made.
        /// Optional field for security tracking.
        /// Helps identify suspicious activities.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Additional context or reason for the change.
        /// Example: "Approved per manager request", "Auto-rejected - expired"
        /// Max 500 characters.
        /// </summary>
        public string? Reason { get; set; }
    }
}

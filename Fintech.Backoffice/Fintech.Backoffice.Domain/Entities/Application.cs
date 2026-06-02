using Fintech.Backoffice.Domain.Common;
using Fintech.Backoffice.Domain.Enums;

namespace Fintech.Backoffice.Domain.Entities
{
    /// <summary>
    /// Represents a loan or invoice application submitted through the platform.
    /// This is the core entity that tracks all application-related data.
    /// Why: This entity is the central hub containing all business metrics and KPIs.
    /// The relationships to Customer and Partner enable dimensional analysis.
    /// </summary>
    public class Application : BaseEntity
    {
        /// <summary>
        /// Unique process number for this application.
        /// This is a business identifier (unlike Id which is database identifier).
        /// Typically formatted like "APP-2024-001234" or similar.
        /// This field must be unique across all applications.
        /// Why: Business users reference applications by ProcessNumber, not database ID.
        /// </summary>
        public string ProcessNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to the Customer entity.
        /// Every application must belong to exactly one customer.
        /// Why: This enables queries like "get all applications from customer X"
        /// and allows data normalization.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Navigation property to the Customer entity.
        /// This provides object-oriented access to the related customer.
        /// Why: Allows code like application.Customer.CustomerName instead of
        /// joining tables manually.
        /// </summary>
        public virtual Customer? Customer { get; set; }

        /// <summary>
        /// Foreign key to the Partner entity.
        /// Every application is submitted by exactly one partner.
        /// Why: Enables partner-wise analytics and reporting.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Navigation property to the Partner entity.
        /// Provides object-oriented access to the related partner.
        /// </summary>
        public virtual Partner? Partner { get; set; }

        /// <summary>
        /// The amount approved for this application (in currency).
        /// Stored as decimal for financial accuracy (not float).
        /// Example: 50000.00 for 50,000 units of currency.
        /// Why: Decimal is used for monetary values to avoid floating-point precision issues.
        /// </summary>
        public decimal ApprovedAmount { get; set; }

        /// <summary>
        /// Current status of the application.
        /// Values: InProgress, Rejected, Completed, Cancelled
        /// Why: Using enum ensures type safety and prevents invalid values.
        /// The integer value (1,2,3,4) is stored in database for better performance.
        /// </summary>
        public ApplicationStatus Status { get; set; } = ApplicationStatus.InProgress;

        /// <summary>
        /// Date when the application was submitted.
        /// Different from CreatedDate: ApplicationDate is the business date of submission.
        /// CreatedDate is when the record was created in the system.
        /// Why: Application processing might happen days after submission,
        /// so we track both dates for accurate analytics.
        /// </summary>
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Optional field for storing processing notes or remarks.
        /// Max 2000 characters - allows storing detailed processing information.
        /// Example: "Approved with conditions" or "Rejected - documents incomplete"
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Completion date - when the application was fully processed.
        /// Optional because in-progress applications won't have this set.
        /// Useful for calculating processing time metrics.
        /// </summary>
        public DateTime? CompletedDate { get; set; }
    }
}

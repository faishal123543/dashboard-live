namespace Fintech.Backoffice.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Application entity.
    /// Used to transfer application data between layers (API responses, database operations).
    /// Why: DTOs decouple database entities from API/UI. Entities can change structure
    /// without affecting external contracts. DTOs also allow selective field exposure for security.
    /// </summary>
    public class ApplicationDto
    {
        /// <summary>
        /// Unique identifier for the application (database ID).
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Business process number - unique business identifier.
        /// Example: "APP-2024-001234"
        /// </summary>
        public string ProcessNumber { get; set; } = string.Empty;

        /// <summary>
        /// Customer ID - foreign key to customer.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer name (populated via JOIN in queries).
        /// Read-only in API - customer is referenced by ID.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Customer mobile number.
        /// </summary>
        public string CustomerMobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Partner ID - foreign key to partner.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Partner name (populated via JOIN).
        /// </summary>
        public string PartnerName { get; set; } = string.Empty;

        /// <summary>
        /// Partner code - unique identifier.
        /// </summary>
        public string PartnerCode { get; set; } = string.Empty;

        /// <summary>
        /// Approved amount in currency.
        /// </summary>
        public decimal ApprovedAmount { get; set; }

        /// <summary>
        /// Current status as integer (1=InProgress, 2=Rejected, 3=Completed, 4=Cancelled).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Status display name (InProgress, Rejected, Completed, Cancelled).
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Date application was submitted.
        /// </summary>
        public DateTime ApplicationDate { get; set; }

        /// <summary>
        /// Processing notes or remarks.
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// Date application was completed.
        /// Null for in-progress applications.
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// System creation date/time.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// System last modified date/time.
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}

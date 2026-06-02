using Fintech.Backoffice.Domain.Common;

namespace Fintech.Backoffice.Domain.Entities
{
    /// <summary>
    /// Represents a partner organization that submits applications through the platform.
    /// Partners are third-party entities like banks, fintech companies, or loan providers.
    /// Why: Separating partners allows multi-tenant analytics and partner performance tracking.
    /// </summary>
    public class Partner : BaseEntity
    {
        /// <summary>
        /// Full name/display name of the partner organization.
        /// Example: "ABC Bank Ltd", "XYZ Fintech"
        /// Max 200 characters.
        /// </summary>
        public string PartnerName { get; set; } = string.Empty;

        /// <summary>
        /// Unique code/identifier for the partner.
        /// Used for integration and identification purposes.
        /// Example: "PARTNER001", "ABC_BANK"
        /// Max 50 characters, usually uppercase alphanumeric.
        /// </summary>
        public string PartnerCode { get; set; } = string.Empty;

        /// <summary>
        /// Contact email address for the partner.
        /// Optional field for communication.
        /// </summary>
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Contact phone number for the partner.
        /// Optional field for communication.
        /// </summary>
        public string? ContactPhone { get; set; }

        /// <summary>
        /// Navigation property to all applications submitted by this partner.
        /// This creates a one-to-many relationship: one partner can submit many applications.
        /// </summary>
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}

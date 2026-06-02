using Fintech.Backoffice.Domain.Common;

namespace Fintech.Backoffice.Domain.Entities
{
    /// <summary>
    /// Represents a customer who submits applications.
    /// This is a dimension entity that holds customer master data.
    /// Why: Separating customer data from applications allows for data normalization,
    /// reduces redundancy, and makes it easy to analyze customer-level metrics.
    /// </summary>
    public class Customer : BaseEntity
    {
        /// <summary>
        /// Full name of the customer.
        /// Required field - max 200 characters.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Mobile phone number of the customer.
        /// Typically stored as a string to preserve formatting and leading zeros.
        /// Max 20 characters to accommodate international formats.
        /// </summary>
        public string MobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the customer.
        /// Optional field for contact purposes.
        /// Max 255 characters as per email standards.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Navigation property to all applications submitted by this customer.
        /// This creates a one-to-many relationship: one customer can have many applications.
        /// Why: Using navigation properties enables LINQ queries like customer.Applications
        /// and allows EF Core to manage relationships automatically.
        /// </summary>
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}

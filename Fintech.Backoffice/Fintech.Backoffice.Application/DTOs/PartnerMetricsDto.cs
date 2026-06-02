namespace Fintech.Backoffice.Application.DTOs
{
    /// <summary>
    /// DTO for partner-wise application metrics.
    /// Shows performance statistics for each partner.
    /// Used in partner analytics dashboard and reporting.
    /// </summary>
    public class PartnerMetricsDto
    {
        /// <summary>
        /// Unique identifier for the partner.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Partner organization name.
        /// </summary>
        public string PartnerName { get; set; } = string.Empty;

        /// <summary>
        /// Unique partner code/identifier.
        /// </summary>
        public string PartnerCode { get; set; } = string.Empty;

        /// <summary>
        /// Total applications submitted by this partner.
        /// </summary>
        public int TotalApplications { get; set; }

        /// <summary>
        /// Count of in-progress applications from this partner.
        /// </summary>
        public int InProgressCount { get; set; }

        /// <summary>
        /// Count of rejected applications from this partner.
        /// </summary>
        public int RejectedCount { get; set; }

        /// <summary>
        /// Count of completed/approved applications from this partner.
        /// </summary>
        public int CompletedCount { get; set; }

        /// <summary>
        /// Count of cancelled applications from this partner.
        /// </summary>
        public int CancelledCount { get; set; }

        /// <summary>
        /// Total approved amount from this partner's applications.
        /// Only counts completed applications.
        /// </summary>
        public decimal TotalApprovedAmount { get; set; }

        /// <summary>
        /// Approval rate for this partner.
        /// Calculated as: (CompletedCount / TotalApplications) * 100
        /// Indicates partner's application quality.
        /// </summary>
        public decimal ApprovalRatePercentage { get; set; }

        /// <summary>
        /// Rejection rate for this partner.
        /// Calculated as: (RejectedCount / TotalApplications) * 100
        /// Higher rejection rate indicates quality issues.
        /// </summary>
        public decimal RejectionRatePercentage { get; set; }

        /// <summary>
        /// Average approved amount per completed application from this partner.
        /// Shows typical value of approvals from this partner.
        /// </summary>
        public decimal AverageApprovedAmount { get; set; }
    }
}

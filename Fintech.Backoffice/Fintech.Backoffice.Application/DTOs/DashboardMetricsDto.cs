namespace Fintech.Backoffice.Application.DTOs
{
    /// <summary>
    /// DTO for dashboard metrics and KPIs.
    /// Contains all the key performance indicators displayed on the dashboard.
    /// Why: Separates dashboard-specific data from individual entity DTOs.
    /// This allows efficient querying of just the metrics needed.
    /// </summary>
    public class DashboardMetricsDto
    {
        // ============================================================================
        // Total Count Metrics
        // ============================================================================

        /// <summary>
        /// Total number of applications in the system (all statuses).
        /// </summary>
        public int TotalApplications { get; set; }

        /// <summary>
        /// Count of applications currently in progress.
        /// </summary>
        public int InProgressCount { get; set; }

        /// <summary>
        /// Count of rejected applications.
        /// </summary>
        public int RejectedCount { get; set; }

        /// <summary>
        /// Count of completed/approved applications.
        /// </summary>
        public int CompletedCount { get; set; }

        /// <summary>
        /// Count of cancelled applications.
        /// </summary>
        public int CancelledCount { get; set; }

        // ============================================================================
        // Financial Metrics
        // ============================================================================

        /// <summary>
        /// Total approved amount from applications approved today.
        /// Only includes applications with status = Completed.
        /// </summary>
        public decimal TodayApprovedAmount { get; set; }

        /// <summary>
        /// Total approved amount for current month.
        /// Rolling up all completed applications in the month.
        /// </summary>
        public decimal MonthlyApprovedAmount { get; set; }

        /// <summary>
        /// Percentage of applications that were rejected.
        /// Calculated as: (RejectedCount / TotalApplications) * 100
        /// </summary>
        public decimal RejectionPercentage { get; set; }

        /// <summary>
        /// Average approved amount per completed application.
        /// Calculated as: TotalApprovedAmount / CompletedCount
        /// Used to analyze application value trends.
        /// </summary>
        public decimal AverageApprovedAmount { get; set; }

        // ============================================================================
        // Time-based Metrics
        // ============================================================================

        /// <summary>
        /// Average processing time in days.
        /// Calculated from ApplicationDate to CompletedDate for completed applications.
        /// </summary>
        public int AverageProcessingDays { get; set; }

        /// <summary>
        /// Count of applications modified in the last 7 days.
        /// Indicates recent activity level.
        /// </summary>
        public int RecentActivityCount { get; set; }

        // ============================================================================
        // Calculated/Derived Metrics
        // ============================================================================

        /// <summary>
        /// Success rate percentage.
        /// Calculated as: (CompletedCount / TotalApplications) * 100
        /// Shows overall approval rate.
        /// </summary>
        public decimal SuccessRatePercentage { get; set; }

        /// <summary>
        /// Date these metrics were calculated.
        /// Useful for caching and cache invalidation.
        /// </summary>
        public DateTime CalculatedDateTime { get; set; } = DateTime.UtcNow;
    }
}

using Fintech.Backoffice.Application.DTOs;

namespace Fintech.Backoffice.Application.ViewModels
{
    /// <summary>
    /// ViewModel for the Dashboard view.
    /// Encapsulates all data needed to render the dashboard page.
    /// Why: ViewModel separates what the view needs from what the service returns.
    /// If we change the dashboard UI, we only modify this class and the view,
    /// not the service or controller.
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Key metrics and KPIs displayed as cards at the top of the dashboard.
        /// </summary>
        public DashboardMetricsDto? Metrics { get; set; }

        /// <summary>
        /// List of recent applications (last 10-20).
        /// Displayed in a table below the metrics.
        /// </summary>
        public List<ApplicationDto> RecentApplications { get; set; } = new();

        /// <summary>
        /// Partner-wise metrics for the partner analytics section.
        /// Used to display partner performance comparison.
        /// </summary>
        public List<PartnerMetricsDto> PartnerMetrics { get; set; } = new();

        /// <summary>
        /// Data for daily application trend chart.
        /// Key = Date, Value = Count
        /// </summary>
        public Dictionary<string, int> DailyApplications { get; set; } = new();

        /// <summary>
        /// Status distribution data for doughnut chart.
        /// Key = Status name, Value = Count
        /// </summary>
        public Dictionary<string, int> StatusDistribution { get; set; } = new();

        /// <summary>
        /// Monthly approved amounts for bar chart.
        /// Key = Month (e.g., "Jan", "Feb"), Value = Amount
        /// </summary>
        public Dictionary<string, decimal> MonthlyApprovedAmounts { get; set; } = new();

        /// <summary>
        /// Rejection rate data: Key = Status, Value = Percentage
        /// For pie chart showing Rejected vs Completed breakdown.
        /// </summary>
        public Dictionary<string, int> RejectionStats { get; set; } = new();

        /// <summary>
        /// Timestamp when dashboard data was last refreshed.
        /// Useful for display "Last updated: 2 minutes ago"
        /// </summary>
        public DateTime LastRefreshed { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// List of available partners for filtering.
        /// Used to populate filter dropdown in the view.
        /// </summary>
        public List<PartnerDto> AvailablePartners { get; set; } = new();

        /// <summary>
        /// Currently selected partner ID filter (if any).
        /// 0 or null means no filter applied.
        /// </summary>
        public int? SelectedPartnerId { get; set; }

        /// <summary>
        /// Date range filter start date.
        /// </summary>
        public DateTime? FilterStartDate { get; set; }

        /// <summary>
        /// Date range filter end date.
        /// </summary>
        public DateTime? FilterEndDate { get; set; }
    }

    /// <summary>
    /// Simple DTO for partner dropdown in filter.
    /// Only includes ID and Name (no full metrics).
    /// </summary>
    public class PartnerDto
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; } = string.Empty;
        public string PartnerCode { get; set; } = string.Empty;
    }
}
